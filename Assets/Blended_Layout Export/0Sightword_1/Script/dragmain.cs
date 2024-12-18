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

    public void Start()
    {
        OBJ_dragmain = this;
        I_Qcount = 0;
        G_final.SetActive(false);
        THI_ShowQuestion();
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

    public void THI_wrg()
    {
        AS_wrg.Play();
    }
    
    public void BUT_Next()
    {
        if(I_Qcount<GA_Questions.Length-1)
        {
            I_Qcount++;
            THI_ShowQuestion();
        }
        else
        {
            G_final.SetActive(true);
        }
    }
}
