using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class introcount : MonoBehaviour
{
    public GameObject[] GA_Objects;
    public GameObject G_final;
    public Text TXT_Max, TXT_Current;
    public int I_Qcount,I_count;
    // Start is called before the first frame update
    void Start()
    {
        I_Qcount = 0;
        G_final.SetActive(false);
       // int j = GA_Objects.Length * 2;
        TXT_Max.text = GA_Objects.Length.ToString();
        int i = I_Qcount + 1;
        TXT_Current.text = i.ToString();
        showquestion();

    }
    public void showquestion()
    {
        for (int i = 0; i < GA_Objects.Length; i++)
        {
            GA_Objects[i].SetActive(false);
        }
        GA_Objects[I_Qcount].SetActive(true);
    }
    public void BUT_next()
    {
        I_Qcount++;
        if (I_Qcount < GA_Objects.Length)
        {
            showquestion();

            int i = I_Qcount + 1;

            TXT_Current.text = i.ToString();
        }
        else
        {
            G_final.SetActive(true);
        }
    }
    public void BUT_Back()
    {
        I_Qcount--;
        if (I_Qcount > -1)
        {
            showquestion();
            int i = I_Qcount + 1;
            TXT_Current.text = i.ToString();
        }
        else
        {
            G_final.SetActive(true);
        }

    }
}
