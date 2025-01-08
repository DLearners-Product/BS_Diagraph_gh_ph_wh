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
#region DataSetter
        // Main_Blended.OBJ_main_blended.levelno = 7;
        QAManager.instance.UpdateActivityQuestion();
        qIndex = 0;
        GetData(I_Qcount);
        GetAdditionalData();
#endregion
        THI_ShowQuestion();
    }

    void THI_ShowQuestion()
    {
        for (int i = 0; i < GA_Questions.Length; i++)
        {
            GA_Questions[i].SetActive(false);
        }
        GA_Questions[I_Qcount].SetActive(true);
        
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
            THI_ShowQuestion();
        }
        else
        {
            BlendedOperations.instance.NotifyActivityCompleted();
            G_final.SetActive(true);
        }
    }

    public void BUT_Back()
    {
        StopPassageVO();
        if (I_Qcount >0)
        {
            I_Qcount--;
            THI_ShowQuestion();
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
            StartCoroutine(PlayRightAudio(G_Selected));
            G_Selected.GetComponent<AudioSource>().Play();
            ScoreManager.instance.RightAnswer(qIndex++, questionID: questions[questionIndex].id, answerID: GetOptionID(selectedOption));
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
