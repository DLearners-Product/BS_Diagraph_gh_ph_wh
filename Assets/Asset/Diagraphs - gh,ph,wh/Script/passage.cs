using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;

public class passage : MonoBehaviour
{
    public GameObject[] GA_Questions;
    public int I_Qcount, I_Count;
    public GameObject G_final, G_Selected;
    public AudioSource AS_Correct, AS_Wrong, AS_Empty;
    public AudioSource AS_header, AS_passage;
    int questionIndex = 0;
    int totalQuestionCount = 6,
        completedQuestionCount = 0;
    public TextMeshProUGUI counterText;
    public GameObject nextBTN;
    public GameObject backBTN;
    int currentIndex = 0;
    List<string> selectedAns = new List<string>();
    Vector3 counterOriginalPosition;
#region QA
    private int qIndex;
    public GameObject questionGO;
    public GameObject[] optionsGO;
    public bool isActivityCompleted = false;
    public Dictionary<string, Component> additionalFields;
    Component[] questions;
    Component[] options;
    Component[] answers;
#endregion

    void Start()
    {
        I_Qcount = 0;
        G_final.SetActive(false);
        counterOriginalPosition = counterText.transform.parent.position;
#region DataSetter
        // Main_Blended.OBJ_main_blended.levelno = 7;
        QAManager.instance.UpdateActivityQuestion();
        qIndex = 0;
        GetData(I_Qcount);
        GetAdditionalData();
#endregion
        UpdateCounter();
        backBTN.GetComponent<Button>().interactable = false;
    }

    void THI_ShowQuestion()
    {
        if(currentIndex == 1f){
            if(selectedAns.Count >= 3)
                nextBTN.GetComponent<Button>().interactable = true;
            else
                nextBTN.GetComponent<Button>().interactable = false;
            backBTN.GetComponent<Button>().interactable = true;
        }else if(currentIndex == 0){
            nextBTN.GetComponent<Button>().interactable = true;
            backBTN.GetComponent<Button>().interactable = false;
        }else if(currentIndex == 2){
            if(selectedAns.Count == 6)
                nextBTN.GetComponent<Button>().interactable = true;
            else
                nextBTN.GetComponent<Button>().interactable = false;
        }

        Vector3 endPosition = GA_Questions[currentIndex].transform.position + (Vector3.up * 10f);
        Utilities.Instance.ANIM_Move(GA_Questions[currentIndex].transform, endPosition, 0f,
        callback : 
        () => {
            GA_Questions[currentIndex].SetActive(true);
            // var _endPosition = GA_Questions[currentIndex].transform.position + (Vector3.down * 10);
            Utilities.Instance.ANIM_Move(GA_Questions[currentIndex].transform, Vector3.zero, callback: () => { LowerCounter(); });
        });
    }

    void LowerCounter()
    {
        float distance = Vector3.Distance(counterText.transform.parent.position, counterOriginalPosition);
        if(currentIndex == 0)
        {
            if(distance > 1f)
            {
                Utilities.Instance.ANIM_Move(counterText.transform.parent, counterOriginalPosition);
            }
        }

        if(currentIndex != 1 || distance > 1) return;
        Debug.Log($"currentIndex :: {currentIndex} -- {distance}");

        Vector3 endPos = counterText.transform.parent.position + (Vector3.down * 1.5f);
        Utilities.Instance.ANIM_Move(counterText.transform.parent, endPos);
    }

    public void ReadPassage()
    {
        StartCoroutine(WaitAndPlayPassageAudio());
    }

    IEnumerator WaitAndPlayPassageAudio()
    {
        AS_header.Play();
        // WaitAndPlayPassageAudio(AS_header.clip.length);
        yield return new WaitForSeconds(AS_header.clip.length);
        AS_passage.Play();
    }

    public void BUT_Next()
    {
        StopPassageVO();
        if (I_Qcount < GA_Questions.Length - 1)
        {
            I_Qcount++;
            // THI_ShowQuestion();
            MoveCurrentPanel(GA_Questions[currentIndex++]);
        }
        else
        {
            BlendedOperations.instance.NotifyActivityCompleted();
            G_final.SetActive(true);
        }
    }

    void UpdateCounter()
    {
        counterText.text = $"{completedQuestionCount} / {totalQuestionCount}";
    }

    void MoveCurrentPanel(GameObject currentPanel)
    {
        Vector3 endPosition = currentPanel.transform.position + (Vector3.up * 10);
        Utilities.Instance.ANIM_Move(currentPanel.transform, endPosition, callback: THI_ShowQuestion);
    }

    public void BUT_Back()
    {
        StopPassageVO();
        if (I_Qcount >0)
        {
            I_Qcount--;
            // THI_ShowQuestion();
            MoveCurrentPanel(GA_Questions[currentIndex--]);
        }
        else
        {
            G_final.SetActive(true);
        }
    }

    public void BUT_Clicking()
    {
        G_Selected = EventSystem.current.currentSelectedGameObject;

        Regex htmlRegex = new Regex("<.*?>", RegexOptions. Compiled);

        Debug.Log($"Question Number :: {G_Selected.transform.parent.parent.name}");

        string selectedOption = G_Selected.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        selectedOption = htmlRegex. Replace(selectedOption, string. Empty);
        questionIndex = GetQuestionIndex(G_Selected.transform.parent.parent.name);
        GetData(questionIndex);

        if (G_Selected.tag == "answer")
        {
            // AS_Correct.Play();
            // Invoke(nameof(PlayRightAudio), G_Selected.GetComponent<AudioSource>().clip.length);

            if(selectedAns.Contains(selectedOption)) return;

            StartCoroutine(PlayRightAudio(G_Selected));
            G_Selected.GetComponent<AudioSource>().Play();
            ScoreManager.instance.RightAnswer(qIndex++, questionID: questions[questionIndex].id, answerID: GetOptionID(selectedOption));
            selectedAns.Add(selectedOption);
            completedQuestionCount++;
            UpdateCounter();

            if((completedQuestionCount % 3) == 0) nextBTN.GetComponent<Button>().interactable = true;
        }
        else
        {
            // Invoke(nameof(PlayWrongAudio), G_Selected.GetComponent<AudioSource>().clip.length);
            StartCoroutine(PlayWrongAudio(G_Selected));
            G_Selected.GetComponent<AudioSource>().Play();
            ScoreManager.instance.WrongAnswer(qIndex, questionID: questions[questionIndex].id, answerID: GetOptionID(selectedOption));
            // AS_Wrong.Play();
        }
    }

    int GetQuestionIndex(string questionNum)
    {
        switch (questionNum)
        {
            case "Q1":
                return 0 + ((I_Qcount - 1) * 3);
            case "Q2":
                return 1 + ((I_Qcount - 1) * 3);
            case "Q3":
                return 2 + ((I_Qcount - 1) * 3);
            default:
                return -1;
        }
    }

    void StopPassageVO()
    {
        AS_header.Stop();
        AS_passage.Stop();
    }

    void DisableOptions(GameObject parentObject)
    {
        int childCount = parentObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            parentObject.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    IEnumerator PlayRightAudio(GameObject selectedObj)
    {
        yield return new WaitForSeconds(selectedObj.GetComponent<AudioSource>().clip.length);
        AS_Correct.Play();
        selectedObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.green;
        yield return new WaitForSeconds(1f);
        DisableOptions(selectedObj.transform.parent.gameObject);
    }

    IEnumerator PlayWrongAudio(GameObject selectedObj)
    {
        yield return new WaitForSeconds(selectedObj.GetComponent<AudioSource>().clip.length);
        AS_Wrong.Play();
        selectedObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(1f);
        THI_Off(selectedObj);
    }

    void THI_Off(GameObject selectedObj)
    {
        selectedObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.gray;
    }

#region QA
    int GetOptionID(string selectedOption)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].text == selectedOption)
            {
                return options[i].id;
            }
        }
        return -1;
    }

    void GetData(int questionIndex)
    {
        // question = QAManager.instance.GetQuestionAt(0, questionIndex);
        questions = QAManager.instance.GetAllQuestions(0);
        options = QAManager.instance.GetOption(0, questionIndex);
        answers = QAManager.instance.GetAnswer(0, questionIndex);
    }

    void GetAdditionalData()
    {
        additionalFields = QAManager.instance.GetAdditionalField(0);
    }
#endregion
}
