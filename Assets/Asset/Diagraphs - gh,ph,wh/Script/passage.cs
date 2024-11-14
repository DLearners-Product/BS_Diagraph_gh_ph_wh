using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class passage : MonoBehaviour
{
    public GameObject[] GA_Questions;
    public int I_Qcount, I_Count;
    public GameObject G_final, G_Selected;
    public AudioSource AS_Correct, AS_Wrong, AS_Empty;
    // Start is called before the first frame update
    void Start()
    {
        I_Qcount = 0;
        G_final.SetActive(false);
        THI_ShowQuestion();
    }

    void THI_ShowQuestion()
    {
        for (int i = 0; i < GA_Questions.Length; i++)
        {
            GA_Questions[i].SetActive(false);
        }
        GA_Questions[I_Qcount].SetActive(true);
        
    }
    public void BUT_Next()
    {
        if (I_Qcount < GA_Questions.Length - 1)
        {
            I_Qcount++;
            THI_ShowQuestion();
        }
        else
        {
            G_final.SetActive(true);
        }
    }
    public void BUT_Back()
    {
        if (I_Qcount >0)
        {
            I_Qcount--;
            THI_ShowQuestion();
        }
        else
        {
            G_final.SetActive(true);
        }
    }
    public void BUT_Clicking()
    {
        
            G_Selected = EventSystem.current.currentSelectedGameObject;
            
            if (G_Selected.tag=="answer")
            {
                AS_Correct.Play();
                G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.green;
            }
            else
            {
                G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
                Invoke("THI_Off", 1f);
                AS_Wrong.Play();
            }
        
    }

    void THI_Off()
    {
        
        G_Selected.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.gray;
    }
}
