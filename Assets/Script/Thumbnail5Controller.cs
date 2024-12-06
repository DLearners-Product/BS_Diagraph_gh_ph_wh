using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Thumbnail5Controller : MonoBehaviour
{
    public GameObject spawnParent;
    public GameObject[] puzzleReference;
    public GameObject[] puzzleParts;
    public GameObject[] wrongPuzzleParts;
    public string[] questionText;
    public QuesObjArr[] questionsArr;
    public TextMeshProUGUI questionDisplayText;
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

    void Start()
    {
        ChangeQuestion();
    }

    void SpawnPuzzleObjects()
    {
        int i=0;
        spawnChildIndex.Clear();
        foreach (var optionItem in questionsArr[currentIndex].questionObjects)
        {
            var questionObj = GetPuzzleObj(optionItem.isAnswer);
            Debug.Log($"Right Puzzle Length :: {puzzleObjs.Count} Wrong Puzzle Length :: {wrongPuzzleObjs.Count}");
            var spawnedObj = Instantiate(questionObj, spawnParent.transform);
            spawnedObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = optionItem.questionText;
            var dragScript = spawnedObj.GetComponent<ImageDragandDrop>();
            spawnChildIndex.Add(spawnedObj.name, i++);
        }
    }

    private void OnEnable()
    {
        ImageDragandDrop.onDragStart += OnDragStart;
        ImageDragandDrop.onDragEnd += OnDragEnd;
        ImageDropSlot.onDropInSlot += OnComponentDrop;
    }

    private void OnDisable()
    {
        ImageDragandDrop.onDragStart -= OnDragStart;
        ImageDragandDrop.onDragEnd -= OnDragEnd;
        ImageDropSlot.onDropInSlot -= OnComponentDrop;
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
        // Debug.Log($"Dropped Game Object :: {dropObj.name} DropSlot :: {dropSlot.name}  {puzzleMathced}");
        if(puzzleMathced)
        {
            dropSlot.GetComponent<Image>().color = Color.white;
            dropSlot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dropObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            Destroy(dropObj);
            answerCount++;
        }

        // Debug.Log($"{answerCount} == {currentCrctAnsCount} {answerCount == currentCrctAnsCount}");
        if(answerCount == currentCrctAnsCount)
        {
            NextQuestion();
        }
    }

    void ChangeQuestion()
    {
        DeleteSpawnedPuzzleObjs();
        ResetPuzzleComponent();
        InstantiatePuzzleQueue();
        questionDisplayText.text = questionText[currentIndex];
        SpawnPuzzleObjects();
        currentIndex++;
    }

    void NextQuestion()
    {
        StartCoroutine(TransitionOn());
        ChangeQuestion();
    }

    IEnumerator TransitionOn()
    {
        transitionAnimation.gameObject.SetActive(true);
        transitionAnimation.Play("ques_transition_in");
        yield return new WaitForSeconds(questionTranIn.length + 1f);
        transitionAnimation.Play("ques_transition_out");
        yield return new WaitForSeconds(questionTranOut.length + 1f);
        transitionAnimation.gameObject.SetActive(false);
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
        while (spawnParent.transform.childCount > 0)
        {
            Destroy(spawnParent.transform.GetChild(0));
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
    public bool isAnswer;
}