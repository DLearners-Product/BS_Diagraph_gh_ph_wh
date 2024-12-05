using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Thumbnail5Controller : MonoBehaviour
{
    public GameObject spawnParent;
    public GameObject[] puzzleParts;
    public GameObject[] wrongPuzzleParts;
    public string[] questionText;
    public QuesObjArr[] questionsArr;
    public TextMeshProUGUI questionDisplayText;
    int currentIndex = 0;
    Queue<GameObject> puzzleObjs;
    Queue<GameObject> wrongPuzzleObjs;
    public GameObject dragObjParent;
    Dictionary<string, int> spawnChildIndex = new Dictionary<string, int>();

    void Start()
    {
        ChangeQuestion();
    }

    void Update()
    {
        
    }

    void SpawnPuzzleObjects()
    {
        int i=0;
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

    private void OnEnable() {
        ImageDragandDrop.onDragStart += OnDragStart;
        ImageDragandDrop.onDragEnd += OnDragEnd;
    }

    private void OnDisable() {
        ImageDragandDrop.onDragStart -= OnDragStart;
        ImageDragandDrop.onDragEnd -= OnDragEnd;
    }

    void OnDragStart(GameObject draggedObj)
    {
        Debug.Log(draggedObj.name);
        draggedObj.transform.SetParent(dragObjParent.transform);
    }

    void OnDragEnd(GameObject dragObject)
    {
        Debug.Log(dragObject.name);
        dragObject.transform.SetParent(spawnParent.transform);
        dragObject.transform.SetSiblingIndex(spawnChildIndex[dragObject.name]);
    }

    void ChangeQuestion()
    {
        InstantiatePuzzleQueue();
        questionDisplayText.text = questionText[currentIndex];
        SpawnPuzzleObjects();
        currentIndex++;
    }

    GameObject GetPuzzleObj(bool isRight)
    {
        return (isRight) ? puzzleObjs.Dequeue() : wrongPuzzleObjs.Dequeue();
    }

    void InstantiatePuzzleQueue()
    {
        puzzleObjs = new Queue<GameObject>(puzzleParts);
        wrongPuzzleObjs = new Queue<GameObject>(wrongPuzzleParts);
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