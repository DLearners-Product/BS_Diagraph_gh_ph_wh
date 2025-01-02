using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Thumbnail5Controller : MonoBehaviour
{
    public GameObject spawnParent;
    public GameObject[] puzzleReference;
    public GameObject[] puzzleParts;
    public GameObject[] wrongPuzzleParts;
    public string[] questionText;
    public QuesObjArr[] questionsArr;
    public AudioClip[] questionAudioClips;
    public TextMeshProUGUI questionDisplayText;
    public AudioClip AC_rightAns;
    public AudioClip AC_wrongAns;
    public AudioSource AS_audioSource;
    int currentIndex = 0;
    int answerCount = 0;
    Queue<GameObject> puzzleObjs;
    Queue<GameObject> wrongPuzzleObjs;
    public GameObject dragObjParent;
    public Animator transitionAnimation;
    public AnimationClip questionTranOut;
    public AnimationClip questionTranIn;
    Dictionary<string, int> spawnChildIndex = new Dictionary<string, int>();
    List<int> excludeInt = new List<int>(){4,0,8};
    QuesObjArr currentQuestionObjs;
    int currentCrctAnsCount = 0;
    AudioClip AC_currentQuesClip;
    public GameObject activityCompleted;

    void Start()
    {
        ChangeQuestion();
        PlayQuestionAudio();
    }

    void SpawnPuzzleObjects()
    {
        int i=0;
        spawnChildIndex.Clear();
        foreach (var optionItem in currentQuestionObjs.questionObjects)
        {
            var questionObj = GetPuzzleObj(optionItem.isAnswer);
            Debug.Log($"Right Puzzle Length :: {puzzleObjs.Count} Wrong Puzzle Length :: {wrongPuzzleObjs.Count}");
            var spawnedObj = Instantiate(questionObj, spawnParent.transform);
            spawnedObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = optionItem.questionText;
            var dragScript = spawnedObj.GetComponent<ImageDragandDrop>();
            spawnedObj.AddComponent<AudioSource>();
            spawnedObj.GetComponent<AudioSource>().clip = optionItem.optionAudioClip;
            spawnChildIndex.Add(spawnedObj.name, i++);
        }
    }

    private void OnEnable()
    {
        ImageDragandDrop.onDragStart += OnDragStart;
        ImageDragandDrop.onDragEnd += OnDragEnd;
        ImageDropSlot.onDropInSlot += OnComponentDrop;
        ImageDropSlot.onMouseHoverEnter += OnMousePointerHover;
        ImageDropSlot.onMouseHoverExit += OnMousePointerExit;
    }

    private void OnDisable()
    {
        ImageDragandDrop.onDragStart -= OnDragStart;
        ImageDragandDrop.onDragEnd -= OnDragEnd;
        ImageDropSlot.onDropInSlot -= OnComponentDrop;
        ImageDropSlot.onMouseHoverEnter -= OnMousePointerHover;
        ImageDropSlot.onMouseHoverExit -= OnMousePointerExit;
    }

    void OnMousePointerHover(GameObject pointerHoldObject, GameObject hoverOnObject)
    {
        if(pointerHoldObject != null && hoverOnObject.GetComponent<Image>().color.a != 1)
        {
            hoverOnObject.GetComponent<Image>().enabled = false;
            hoverOnObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    void OnMousePointerExit(GameObject pointerHoldObject, GameObject hoverOnObject)
    {
        if(pointerHoldObject != null)
        {
            hoverOnObject.GetComponent<Image>().enabled = true;
            hoverOnObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    void OnDragStart(GameObject draggedObj)
    {
        // Debug.Log(draggedObj.name);
        draggedObj.transform.SetParent(dragObjParent.transform);
    }

    void OnDragEnd(GameObject dragObject)
    {
        // Debug.Log(dragObject.name);
        dragObject.transform.SetParent(spawnParent.transform);
        dragObject.transform.SetSiblingIndex(spawnChildIndex[dragObject.name]);
    }

    void OnComponentDrop(GameObject dropObj, GameObject dropSlot)
    {
        bool puzzleMathced = dropObj.name.Replace("(Clone)", "").Trim() == dropSlot.name.Trim();
        float clipLen = 0f;
        // Debug.Log($"Dropped Game Object :: {dropObj.name} DropSlot :: {dropSlot.name}  {puzzleMathced}");
        dropSlot.GetComponent<Image>().enabled = true;
        dropSlot.transform.GetChild(1).gameObject.SetActive(false);
        if(puzzleMathced)
        {
            var puzzleAudio = dropObj.GetComponent<AudioSource>().clip;
            clipLen = puzzleAudio.length;
            AS_audioSource.PlayOneShot(puzzleAudio);
            dropSlot.GetComponent<Image>().color = Color.white;
            dropSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dropObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            // Invoke(, AS_audioSource.clip.length);
            Destroy(dropObj);
            answerCount++;
        }else{
            clipLen = AC_wrongAns.length;
            AS_audioSource.PlayOneShot(AC_wrongAns);
        }

        // Debug.Log($"{answerCount} == {currentCrctAnsCount} {answerCount == currentCrctAnsCount}");
        if(answerCount == currentCrctAnsCount)
        {
            Invoke(nameof(NextQuestion), clipLen + 1f);
        }
    }

    void ChangeQuestion()
    {
        DeleteSpawnedPuzzleObjs();
        ResetPuzzleComponent();
        InstantiatePuzzleQueue();
        questionDisplayText.text = questionText[currentIndex];
        AC_currentQuesClip = questionAudioClips[currentIndex];
        SpawnPuzzleObjects();
        currentIndex++;
    }

    void NextQuestion()
    {
        if(questionText.Length < (currentIndex + 1))
        {
            activityCompleted.SetActive(true);
            return;
        }

        StartCoroutine(TransitionOn());
    }

    IEnumerator TransitionOn()
    {
        transitionAnimation.gameObject.SetActive(true);
        transitionAnimation.Play("ques_transition_in");
        yield return new WaitForSeconds(questionTranIn.length + 1f);
        ChangeQuestion();
        transitionAnimation.Play("ques_transition_out");
        yield return new WaitForSeconds(questionTranOut.length + 1f);
        transitionAnimation.gameObject.SetActive(false);
        PlayQuestionAudio();
    }

    public void PlayQuestionAudio()
    {
        AS_audioSource.PlayOneShot(AC_currentQuesClip);
    }

    GameObject GetPuzzleObj(bool isRight)
    {
        return (isRight) ? puzzleObjs.Dequeue() : wrongPuzzleObjs.Dequeue();
    }

    int GetCorrectAnsCount()
    {
        currentQuestionObjs = questionsArr[currentIndex];
        int rightAnsCount = 0;

        for (int i = 0; i < currentQuestionObjs.questionObjects.Length; i++)
        {
            if(currentQuestionObjs.questionObjects[i].isAnswer)
                rightAnsCount++;
        }

        return rightAnsCount;
    }

    void InstantiatePuzzleQueue()
    {
        currentCrctAnsCount = GetCorrectAnsCount();
        answerCount = 0;
        int unAnswerCount = puzzleParts.Length - currentCrctAnsCount;
        List<GameObject> _puzzleParts = new List<GameObject>(puzzleParts);

        for (int i = 0; i < unAnswerCount; i++)
        {
            puzzleReference[excludeInt[i]].GetComponent<Image>().color = Color.white;
        }

        puzzleObjs = new Queue<GameObject>();
        for (int i = 0; i < puzzleParts.Length; i++)
        {
            if(!excludeInt.GetRange(0,unAnswerCount).Contains(i))
                puzzleObjs.Enqueue(puzzleParts[i]);
        }
        wrongPuzzleObjs = new Queue<GameObject>(wrongPuzzleParts);
    }

    void ResetPuzzleComponent()
    {
        foreach (var puzzle in puzzleReference)
        {
            puzzle.GetComponent<Image>().color = new Color32(255, 255, 255, 91);
            puzzle.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    void DeleteSpawnedPuzzleObjs()
    {
        int childCount = spawnParent.transform.childCount;
        Debug.Log($"Child Count :: {childCount}");
        for (int i = 0; i < childCount; i++)
        {
            var delObj = spawnParent.transform.GetChild(0);
            delObj.SetParent(null);
            Destroy(delObj.gameObject);
            Debug.Log($"Child Count :: {spawnParent.transform.childCount}");
        }
    }
}

[System.Serializable]
public class QuesObjArr
{
    public QuestionObject[] questionObjects;
}

[System.Serializable]
public class QuestionObject
{
    public string questionText;
    public AudioClip optionAudioClip;
    public bool isAnswer;
}