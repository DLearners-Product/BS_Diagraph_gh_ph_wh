using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class dragmain : MonoBehaviour
{
    public static dragmain OBJ_dragmain;
    public GameObject[] GA_Questions;
    public int I_Qcount,I_Count;
    public GameObject  G_final,G_Next, petal;
    public AudioSource AS_crt, AS_wrg;
    public string STR_Selected;
    public Color CLR_Coloor;
    public AudioClip AC_Clip;
    public AudioClip AC_bubblePop;
    public AudioSource AS_emptyAudioSource;
    public TextMeshProUGUI counterText;

#region QA
    private int qIndex;
    public GameObject questionGO;
    public GameObject[] optionsGO;
    public bool isActivityCompleted = false;
    public Dictionary<string, Component> additionalFields;
    Component question;
    Component[] options;
    Component[] answers;
#endregion

    public void Start()
    {
        OBJ_dragmain = this;
        I_Qcount = 0;
        G_final.SetActive(false);
        THI_ShowQuestion();

#region DataSetter
        // Main_Blended.OBJ_main_blended.levelno = 4;
        QAManager.instance.UpdateActivityQuestion();
        qIndex = 0;
        GetData(I_Qcount);
        GetAdditionalData();
#endregion
    }

    void THI_ShowQuestion()
    {
        for (int i=0;i<GA_Questions.Length;i++)
        {
            GA_Questions[i].SetActive(false);
        }
        ShrinkOptions(GA_Questions[I_Qcount]);
        PopUpOptinos(GA_Questions[I_Qcount]);
        GA_Questions[I_Qcount].SetActive(true);
        G_Next.GetComponent<Button>().interactable = false;
        I_Count = 0;
        UpdateCounter();
    }

    public void THI_Correct()
    {
        I_Count++;
        petal = GA_Questions[I_Qcount].transform.GetChild(0).transform.GetChild(0).transform.GetChild(I_Count).gameObject;

        Debug.Log($"Selected Ans :: {STR_Selected}");

        ScoreManager.instance.RightAnswer(qIndex, questionID: question.id, answerID: GetOptionID(STR_Selected));
        qIndex++;

        petal.GetComponent<Image>().color = CLR_Coloor;
        petal.GetComponent<AudioSource>().clip = AC_Clip;
        petal.GetComponent<AudioSource>().Play();
        petal.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = STR_Selected;
        AS_crt.Play();

        if (I_Count == 5)
        {
            G_Next.GetComponent<Button>().interactable = true;
        }
    }

    void ShrinkOptions(GameObject optionParentObj)
    {
        var optionParent = optionParentObj.transform.GetChild(1).GetChild(0);
        for (int i = 0; i < optionParent.childCount; i++)
        {
            Utilities.Instance.ANIM_ShrinkObject(optionParent.GetChild(i), 0f);
            // optionParent.GetChild(i)
        }
    }

    void PopUpOptinos(GameObject questinoObj)
    {
        var optionParent = questinoObj.transform.GetChild(1).GetChild(0);
        var childObjs = GetChildObjs(optionParent);
        Utilities.Instance.ApplyScaleEffectsToChildObjects(childObjs.ToArray());
    }

    void UpdateCounter()
    {
        counterText.text = $"{I_Qcount + 1} / {GA_Questions.Length}";
    }

    List<GameObject> GetChildObjs(Transform parentObj)
    {
        List<GameObject> childObjs = new List<GameObject>();
        int childCount = parentObj.childCount;
        for (int i = 0; i < childCount; i++)
        {
            childObjs.Add(parentObj.GetChild(i).gameObject);
        }
        return childObjs;
    }

    void PlayPopSound(GameObject childObject)
    {
        Debug.Log(childObject.name);
        // childObject.PlayOneShot(AC_bubblePop);
    }

    public void THI_wrg(GameObject selectedObj)
    {
        var selectedOption = selectedObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        ScoreManager.instance.WrongAnswer(qIndex, questionID: question.id, answerID: GetOptionID(selectedOption));
        AS_wrg.Play();
    }
    
    public void BUT_Next()
    {
        if(I_Qcount<GA_Questions.Length-1)
        {
            I_Qcount++;
            THI_ShowQuestion();
            GetData(I_Qcount);
        }
        else
        {
            BlendedOperations.instance.NotifyActivityCompleted();
            G_final.SetActive(true);
        }
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
        question = QAManager.instance.GetQuestionAt(0, questionIndex);
        options = QAManager.instance.GetOption(0, questionIndex);
        answers = QAManager.instance.GetAnswer(0, questionIndex);
    }
 
    void GetAdditionalData()
    {
        additionalFields = QAManager.instance.GetAdditionalField(0);
    }
 
    // void AssignData()
    // {
    //     // Custom code
    //     for (int i = 0; i < optionsGO.Length; i++)
    //     {
    //         optionsGO[i].GetComponent<Image>().sprite = options[i]._sprite;
    //         optionsGO[i].tag = "Untagged";
    //         Debug.Log(optionsGO[i].name, optionsGO[i]);
    //         // if (CheckOptionIsAns(options[i]))
    //         // {
    //         //     optionsGO[i].tag = "answer";
    //         // }
    //     }
    //     // answerCount.text = "/"+answers.Length;
    // }
#endregion
}
