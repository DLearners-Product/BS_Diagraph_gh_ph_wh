﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class listenandclick : MonoBehaviour
{
    public GameObject[] GA_Options;
    public int I_Qcount;
    public GameObject G_final, G_Selected;
    public AudioSource AS_Correct, AS_Wrong, AS_Empty;
    public AudioClip[] AC_Clips;
    public Text TXT_Max, TXT_Current;
    bool B_CanClick;
    // Start is called before the first frame update
    void Start()
    {
        I_Qcount = 0;
        G_final.SetActive(false);
        THI_Speaker();
        TXT_Max.text = AC_Clips.Length.ToString();
        int x = I_Qcount + 1;
        TXT_Current.text = x.ToString();
    }

    void THI_Speaker()
    {
        AS_Empty.clip = AC_Clips[I_Qcount];
        for(int i=0;i<GA_Options.Length;i++)
        {
            GA_Options[i].transform.GetChild(0).GetComponent<Image>().color = Color.gray;
        }
        
    }

    public void BUT_Speaker()
    {
        AS_Empty.Play();
        B_CanClick = true;
    }
    public void BUT_Next()
    {
        if (I_Qcount < AC_Clips.Length - 1)
        {
            I_Qcount++;
            THI_Speaker();
            int x = I_Qcount + 1;
            TXT_Current.text = x.ToString();
        }
        else
        {
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
        if(B_CanClick)
        {
            G_Selected = EventSystem.current.currentSelectedGameObject;
            B_CanClick = false;
            // Debug.Log(G_Selected.name);
            if (G_Selected.name == "Opt1")
            {
                if (I_Qcount == 0 || I_Qcount == 2 || I_Qcount == 7 || I_Qcount == 9)
                {
                    AS_Correct.Play();
                    G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    AS_Wrong.Play();
                    G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
                    Invoke("THI_Off", 1f);
                }
            }
            if (G_Selected.name == "Opt2")
            {
                if (I_Qcount == 1 || I_Qcount == 3 || I_Qcount == 6 || I_Qcount == 8)
                {
                    AS_Correct.Play();
                    G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    AS_Wrong.Play();
                    G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
                    Invoke("THI_Off", 1f);
                }
            }
            if (G_Selected.name == "Opt3")
            {
                if (I_Qcount == 4 || I_Qcount == 5)
                {
                    AS_Correct.Play();
                    G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    AS_Wrong.Play();
                    G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
                    Invoke("THI_Off", 1f);
                }
            }
        }
       

    }

    void THI_Off()
    {
        B_CanClick = true;
        G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.gray;
    }
}