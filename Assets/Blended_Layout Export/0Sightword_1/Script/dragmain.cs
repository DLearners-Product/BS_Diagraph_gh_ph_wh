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
        GA_Questions[I_Qcount].SetActive(true);
        G_Next.GetComponent<Button>().interactable = false;
        I_Count = 0;
        
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
