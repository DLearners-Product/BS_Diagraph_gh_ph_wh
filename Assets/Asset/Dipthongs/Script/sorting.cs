using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class sorting : MonoBehaviour
{
    public TextMeshProUGUI TMP_Que;
    public string[] STR_Questions;
    public int I_Qcount;
    public int I_Count;
    public GameObject G_Final, G_Selected, G_TV, G_Animation,G_Next;
    bool B_CanClick;
    public AudioSource AS_Correct, AS_Wrong,AS_Empty;
    public AudioClip[] AC_Clips;
    public Sprite[] SPR_Images;
    public Text TXT_Max, TXT_Current;
    // Start is called before the first frame update
    void Start()
    {
        I_Qcount = 0;
        THI_ShowAnswer();
        G_Final.SetActive(false);
        TXT_Max.text = STR_Questions.Length.ToString();
        int x = I_Qcount + 1;
        TXT_Current.text = x.ToString();
    }

   /* void THI_ShowQuestion()
    {
       // G_TV.GetComponent<Animator>().Play("tvon");
      //  G_TV.transform.GetChild(0).GetComponent<Image>().sprite = SPR_Images[I_Qcount];
      //  G_TV.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
      //  G_Next.GetComponent<Button>().interactable = false;
      //  B_CanClick = true;
    }*/

    void THI_ShowAnswer()
    {
        TMP_Que.text = STR_Questions[I_Qcount];
        G_Animation.GetComponent<Animator>().Play("boxin");
        G_Animation.transform.GetChild(1).GetComponent<ClickAudio>().clip = AC_Clips[I_Qcount];
        G_Next.GetComponent<Button>().interactable = false;
        AS_Empty.clip = AC_Clips[I_Qcount];
        AS_Empty.Play();
        B_CanClick = true;
    }

    public void BUT_Clicking(int index)
    {
        if (B_CanClick)
        {
            I_Count = index;
            G_Selected = EventSystem.current.currentSelectedGameObject;
            if (STR_Questions[I_Qcount].Contains(G_Selected.name))
            {
                if(I_Count==1)
                {
                   if(! STR_Questions[I_Qcount].Contains("ght"))
                   {
                        B_CanClick = false;
                       // G_Animation.GetComponent<Animator>().Play("boxin");
                        AS_Correct.Play();
                       // THI_ShowAnswer();
                        Invoke("THI_Nexton", 1f);
                   }
                    else
                    {
                        AS_Wrong.Play();
                    }
                }
                else
                {
                    B_CanClick = false;
                  //  G_Animation.GetComponent<Animator>().Play("boxin");
                    AS_Correct.Play();
                   // THI_ShowAnswer();
                    Invoke("THI_Nexton", 1f);
                }
               
            }
            else
            {
                AS_Wrong.Play();
            }
        }
    }

    void THI_Nexton()
    {
        switch (I_Count)
        {
            case 1: G_Animation.GetComponent<Animator>().SetInteger("Cond", I_Count); break;
            case 2: G_Animation.GetComponent<Animator>().SetInteger("Cond", I_Count); break;
            case 3: G_Animation.GetComponent<Animator>().SetInteger("Cond", I_Count); break;
            case 4: G_Animation.GetComponent<Animator>().SetInteger("Cond", I_Count); break;
        }
       // G_TV.GetComponent<Animator>().Play("tvoff");
        G_Next.GetComponent<Button>().interactable = true;
       // AS_Empty.clip = AC_Clips[I_Qcount];
       // AS_Empty.Play();
    }
    public void BUT_Next()
    {
        if(I_Qcount<STR_Questions.Length-1)
        {
            G_Animation.GetComponent<Animator>().Play("New State 0");
            G_Animation.GetComponent<Animator>().SetInteger("Cond", 0);
            I_Qcount++;
            // THI_ShowQuestion();
            THI_ShowAnswer();
            int x = I_Qcount + 1;
            TXT_Current.text = x.ToString();
        }
        else
        {
            G_Final.SetActive(true);
        }
    }
}
