using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class listenandclick : MonoBehaviour
{
    public GameObject[] GA_Options;
    public int I_Qcount;
    public GameObject G_final, G_Selected;
    public Animator ANIMT_speaker;
    public AudioSource AS_Correct, AS_Wrong, AS_Empty;
    public AudioClip[] AC_Clips;
    public Text TXT_Max, TXT_Current;
    public AnimationClip speakerClip;
    bool B_CanClick;
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

    void Start()
    {
        I_Qcount = 0;
        B_CanClick = true;
        G_final.SetActive(false);
        THI_Speaker();
        TXT_Max.text = AC_Clips.Length.ToString();
        int x = I_Qcount + 1;
        TXT_Current.text = x.ToString();
        SpawnOption();
#region DataSetter
        Main_Blended.OBJ_main_blended.levelno = 8;
        QAManager.instance.UpdateActivityQuestion();
        qIndex = 0;
        GetData(I_Qcount);
        GetAdditionalData();
#endregion
    }

    void SpawnOption()
    {
        Utilities.Instance.ApplyScaleEffectsToChildObjects(GA_Options);
    }

    void THI_Speaker()
    {
        AS_Empty.clip = AC_Clips[I_Qcount];
        AS_Empty.Play();
        ANIMT_speaker.Play("speaker_anim");

        for(int i=0;i<GA_Options.Length;i++)
        {
            GA_Options[i].transform.GetChild(0).GetComponent<Image>().color = Color.gray;
        }
        
    }

    public void BUT_Speaker()
    {
        Debug.Log("Came to BUT_Speaker()");
        // var currentObjAnimator = EventSystem.current.currentSelectedGameObject.GetComponent<Animator>();
        ANIMT_speaker.Play("speaker_anim");
        AS_Empty.Play();
        B_CanClick = true;
    }

    public void BUT_Next()
    {
        B_CanClick = true;
        if (I_Qcount < AC_Clips.Length - 1)
        {
            SpawnOption();
            I_Qcount++;
            GetData(I_Qcount);
            Invoke(nameof(THI_Speaker), 1.5f);
            int x = I_Qcount + 1;
            TXT_Current.text = x.ToString();
        }
        else
        {
            BlendedOperations.instance.NotifyActivityCompleted();
            G_final.SetActive(true);
        }
    }

    public void BUT_Back()
    {
        if (I_Qcount > 0)
        {
            I_Qcount--;
            int x = I_Qcount + 1;
            TXT_Current.text = x.ToString();
            THI_Speaker();
        }
        else
        {
            G_final.SetActive(true);
        }
    }

    public void BUT_Clicking()
    {
        Debug.Log($"B_CanClick :: {B_CanClick}");
        if(!B_CanClick) return;

        G_Selected = EventSystem.current.currentSelectedGameObject;
        B_CanClick = false;
        // Debug.Log(G_Selected.name);
        if (G_Selected.name == "Opt1")
        {
            if (I_Qcount == 0 || I_Qcount == 2 || I_Qcount == 7 || I_Qcount == 9)
            {
                AS_Correct.Play();
                Utilities.Instance.ScaleObject(G_Selected.transform, 1.15f, 0.5f);
                Invoke(nameof(BUT_Next), AS_Correct.clip.length);
                ScoreManager.instance.RightAnswer(qIndex++, questionID: question.id, answerID: GetOptionID("gh"));
                // G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.green;
            }
            else
            {
                AS_Wrong.Play();
                G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
                ScoreManager.instance.WrongAnswer(qIndex, questionID: question.id, answerID: GetOptionID("gh"));
                Invoke("THI_Off", AS_Wrong.clip.length);
            }
        }
        if (G_Selected.name == "Opt2")
        {
            if (I_Qcount == 1 || I_Qcount == 6 || I_Qcount == 8)
            {
                AS_Correct.Play();
                Utilities.Instance.ScaleObject(G_Selected.transform, 1.15f, 0.5f);
                Invoke(nameof(BUT_Next), AS_Correct.clip.length);
                ScoreManager.instance.RightAnswer(qIndex++, questionID: question.id, answerID: GetOptionID("ph"));
                // G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.green;
            }
            else
            {
                AS_Wrong.Play();
                G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
                ScoreManager.instance.WrongAnswer(qIndex, questionID: question.id, answerID: GetOptionID("ph"));
                Invoke("THI_Off", AS_Wrong.clip.length);
            }
        }
        if (G_Selected.name == "Opt3")
        {
            if (I_Qcount == 3 || I_Qcount == 4 || I_Qcount == 5)
            {
                AS_Correct.Play();
                Utilities.Instance.ScaleObject(G_Selected.transform, 1.15f, 0.5f);
                Invoke(nameof(BUT_Next), AS_Correct.clip.length);
                ScoreManager.instance.RightAnswer(qIndex++, questionID: question.id, answerID: GetOptionID("wh"));
                // G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.green;
            }
            else
            {
                AS_Wrong.Play();
                G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
                ScoreManager.instance.WrongAnswer(qIndex, questionID: question.id, answerID: GetOptionID("wh"));
                Invoke("THI_Off", AS_Wrong.clip.length);
            }
        }
    }

    void THI_Off()
    {
        B_CanClick = true;
        G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.gray;
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
#endregion
}