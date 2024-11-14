using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class recall : MonoBehaviour
{
    public Sprite[] SPR_Pics;
    public Image IMG_Main;
    public Text TXT_Max, TXT_Current;
    public int I_Qcount;
    public GameObject G_Final;
    void Start()
    {
        I_Qcount = 0;
        THI_ShowQuestion();
        G_Final.SetActive(false);
        TXT_Max.text = SPR_Pics.Length.ToString();
        int x = I_Qcount + 1;
        TXT_Current.text = x.ToString();
    }
    void THI_ShowQuestion()
    {
        IMG_Main.sprite = SPR_Pics[I_Qcount];
        IMG_Main.preserveAspect = true;
    }
    // chin, ship, cherry, shell, thorn, wash, push, sheep, bath, watch

    public void BUT_Next()
    {
        if(I_Qcount<SPR_Pics.Length-1)
        {
            I_Qcount++; 
            int x = I_Qcount + 1;
            TXT_Current.text = x.ToString();
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
            int x = I_Qcount + 1;
            TXT_Current.text = x.ToString();
            THI_ShowQuestion();
        }
        else
        {
            G_Final.SetActive(true);
        }
    }
}
