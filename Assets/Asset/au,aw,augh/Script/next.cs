using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class next : MonoBehaviour
{
    public GameObject[] GA_Questions, G_Examples;
    public int I_Qcount;
    public Transform postionTransform;
    public AudioSource emptyAudioSource;
    public GameObject seaWave;
    public GameObject[] diagraphsObjects;
    public GameObject[] digraphAnswers;
    public AudioClip[] questionClip;
    public AudioClip[] answerClip;
    public GameObject G_Final;
    GameObject G_Selected;
    bool B_CanClick;
    GameObject currentEnabledObj = null;
    int currentDiagINDX = 0;
    // Start is called before the first frame update
    void Start()
    {
        I_Qcount = 0;
        currentDiagINDX = 0;
        THI_ShowQuestion();
        G_Final.SetActive(false);
        THI_Off();
        currentEnabledObj = diagraphsObjects[currentDiagINDX];
    }

    public void BUT_Speaker()
    {
        if(B_CanClick)
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().Play();
            G_Selected = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.gameObject;
            var clip = EventSystem.current.currentSelectedGameObject.GetComponent<AudioSource>().clip;
            B_CanClick = false;
            Invoke("THI_ShowFishes", clip.length);
            if(I_Qcount==3)
            {
                Invoke("THI_Ph", clip.length);
            }
        }
    }

    void THI_Ph()
    {
        for (int i = 2; i < GA_Questions[I_Qcount].transform.childCount; i++)
        {
            Debug.Log(GA_Questions[I_Qcount].transform.GetChild(i).transform.GetChild(0).transform.GetChild(1).name);
            GA_Questions[I_Qcount].transform.GetChild(i).transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
            GA_Questions[I_Qcount].transform.GetChild(i).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    void THI_ShowFishes()
    {
        G_Selected.transform.GetChild(1).gameObject.SetActive(true);
        Invoke("THI_ShowNet", 1f);
    }
    void THI_ShowNet()
    {
        G_Selected.transform.GetChild(2).gameObject.SetActive(true);
        B_CanClick = true;
    }

    void THI_ShowQuestion()
    {
        for(int i=0;i<GA_Questions.Length;i++)
        {
            GA_Questions[i].SetActive(false);
        }
        GA_Questions[I_Qcount].SetActive(true);
        if(I_Qcount>0)
        {
            for (int i = 2; i < GA_Questions[I_Qcount].transform.childCount; i++)
            {
              //  Debug.Log(GA_Questions[I_Qcount].transform.GetChild(i).transform.GetChild(0).transform.GetChild(1).name);
                GA_Questions[I_Qcount].transform.GetChild(i).transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                GA_Questions[I_Qcount].transform.GetChild(i).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        B_CanClick = true;
    }

    public void BUT_ShowLetters(int index)
    {
        for (int i = 0; i < G_Examples.Length; i++)
        {
            G_Examples[i].SetActive(false);
        }
        G_Examples[index].SetActive(true);
        Invoke("THI_Off", 2f);
    }

    void THI_Off()
    {

        for (int i = 0; i < G_Examples.Length; i++)
        {
            G_Examples[i].SetActive(false);
        }
    }
    public void BUT_Next()
    {
       // Debug.Log("Next 1");
        if (I_Qcount < GA_Questions.Length-1)
        {
            //Debug.Log("Next");
            I_Qcount++;
            THI_ShowQuestion();
        }
        else
        {
            G_Final.SetActive(true);
        }
    }

    public void BUT_Back()
    {
        if (I_Qcount > 0)
        {
            I_Qcount--;
            THI_ShowQuestion();
        }
        else
        {
            G_Final.SetActive(true);
        }
    }

    public void OnShellClicked()
    {
        var clickedObj = EventSystem.current.currentSelectedGameObject;
        string selectedSTR = clickedObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        var  digraphIndx = GetDigraphObjectIndex(selectedSTR);

        Utilities.Instance.ANIM_MoveUpDown(seaWave.transform, postionTransform.position, () => {EnableNextDigraph(digraphIndx);});
    }

    public void EnableNextDigraph(int enableIndex)
    {
        currentEnabledObj.SetActive(false);
        currentDiagINDX = enableIndex;
        emptyAudioSource.PlayOneShot(questionClip[currentDiagINDX]);
        Invoke(nameof(PlayAnswer), questionClip[currentDiagINDX].length);
        currentEnabledObj = diagraphsObjects[enableIndex];
        currentEnabledObj.SetActive(true);
    }

    void PlayAnswer()
    {
        digraphAnswers[currentDiagINDX].SetActive(true);
        emptyAudioSource.PlayOneShot(answerClip[currentDiagINDX]);
    }

    int GetDigraphObjectIndex(string diagraphSTR)
    {
        for (int i = 0; i < diagraphsObjects.Length; i++)
        {
            if(diagraphsObjects[i].name.Trim() == diagraphSTR.Trim())
                return i;
        }
        return -1;
    }
}
