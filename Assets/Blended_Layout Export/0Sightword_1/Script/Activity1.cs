using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Activity1 : MonoBehaviour
{
    public GameObject[] GA_Objects;
    public int I_count,I_dummy;
    public GameObject G_final,G_Toogle;
    public Text TXT_Max, TXT_Current;

    // Start is called before the first frame update
    void Start()
    {
        I_count = 0;
        G_final.SetActive(false);
        TXT_Max.text = GA_Objects.Length.ToString();
        int i = I_count + 1;
        TXT_Current.text = i.ToString();
        showquestion();
    }
    public void showquestion()
    {
        for (int i = 0; i < GA_Objects.Length; i++)
        {
            GA_Objects[i].SetActive(false);
        }
        GA_Objects[I_count].SetActive(true);
       // GA_Objects[I_count].transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
       // GA_Objects[I_count].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        for(int i=0;i<G_Toogle.transform.childCount;i++)
        {
            G_Toogle.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
        }
        I_dummy = 0;
    }
    
    public void BUT_Vowelchange()
    {
        GA_Objects[I_count].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
        GA_Objects[I_count].transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
    }
    public void BUT_cardflip()
    {
        
        I_dummy++;
        if (I_dummy % 2 != 0)
        {
            GA_Objects[I_count].transform.GetChild(0).GetComponent<Animator>().Play("cardflip1");
        }
        else
        {
            GA_Objects[I_count].transform.GetChild(0).GetComponent<Animator>().Play("reverseflip");
        }
    }
    public void BUT_next()
    {
        I_count++;
        if(I_count<GA_Objects.Length)
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
    public void BUT_Back()
    {
        I_count--;
        if (I_count > -1)
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
