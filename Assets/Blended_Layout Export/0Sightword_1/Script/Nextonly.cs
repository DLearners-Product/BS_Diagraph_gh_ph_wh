using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nextonly : MonoBehaviour
{
    public static Nextonly OBJ_Nextonly;
    public GameObject[] GA_Questions;
    public AudioSource AS_Empty, AS_Wrong, AS_Correct;
    public AudioClip[] AC_clips;
    public int I_count,I_Dummy;
    public GameObject G_final;
    public Text TXT_Max, TXT_Current;

    // Start is called before the first frame update
    void Start()
    {
        OBJ_Nextonly = this;
        G_final.SetActive(false);
        I_count = 0;
        TXT_Max.text = GA_Questions.Length.ToString();
        int i = I_count + 1;
        TXT_Current.text = i.ToString();
        showquestion();
    }
    public void showquestion()
    {
        for(int i=0;i<GA_Questions.Length;i++)
        {
            GA_Questions[i].SetActive(false);
        }
        GA_Questions[I_count].SetActive(true);
        GA_Questions[I_count].transform.GetChild(1).gameObject.SetActive(true);
        GA_Questions[I_count].transform.GetChild(2).gameObject.SetActive(false);
    }
    // Update is called once per frame
    public void BUT_Speaker()
    {
        AS_Empty.clip = AC_clips[I_count];
        AS_Empty.Play();
    }
    public void THI_Wrong()
    {
        AS_Wrong.Play();
    }
    public void THI_Correct()
    {
        AS_Correct.Play();
        if (I_count == 1)
        {
            I_Dummy++;
            if (I_Dummy == 2)
            {
                GA_Questions[I_count].transform.GetChild(1).gameObject.SetActive(false);
                GA_Questions[I_count].transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        else
        {
            GA_Questions[I_count].transform.GetChild(1).gameObject.SetActive(false);
            GA_Questions[I_count].transform.GetChild(2).gameObject.SetActive(true);
        }
       
    }
    public void BUT_next()
    {
        I_count++;
        if(I_count<GA_Questions.Length)
        {
            showquestion();
            int i = I_count + 1;
            TXT_Current.text = i.ToString();
        }
        else
        {
            G_final.SetActive(true);
        }
    }
}
