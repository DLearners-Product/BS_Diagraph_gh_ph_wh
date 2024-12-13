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
    public AudioSource AS_header, AS_passage;

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

    public void ReadPassage()
    {
        StartCoroutine(WaitAndPlayPassageAudio());
    }

    IEnumerator WaitAndPlayPassageAudio()
    {
        AS_header.Play();
        // WaitAndPlayPassageAudio(AS_header.clip.length);
        yield return new WaitForSeconds(AS_header.clip.length);
        AS_passage.Play();
    }

    public void BUT_Next()
    {
        StopPassageVO();
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
        StopPassageVO();
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
            // AS_Correct.Play();
            // Invoke(nameof(PlayRightAudio), G_Selected.GetComponent<AudioSource>().clip.length);
            StartCoroutine(PlayRightAudio(G_Selected));
            G_Selected.GetComponent<AudioSource>().Play();
        }
        else
        {
            // Invoke(nameof(PlayWrongAudio), G_Selected.GetComponent<AudioSource>().clip.length);
            StartCoroutine(PlayWrongAudio(G_Selected));
            G_Selected.GetComponent<AudioSource>().Play();
            // AS_Wrong.Play();
        }
    }

    void StopPassageVO()
    {
        AS_header.Stop();
        AS_passage.Stop();
    }

    IEnumerator PlayRightAudio(GameObject selectedObj)
    {
        yield return new WaitForSeconds(selectedObj.GetComponent<AudioSource>().clip.length);
        AS_Correct.Play();
        selectedObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.green;
    }

    IEnumerator PlayWrongAudio(GameObject selectedObj)
    {
        yield return new WaitForSeconds(selectedObj.GetComponent<AudioSource>().clip.length);
        AS_Wrong.Play();
        selectedObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(1f);
        THI_Off(selectedObj);
    }

    void THI_Off(GameObject selectedObj)
    {
        selectedObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.gray;
    }
}
