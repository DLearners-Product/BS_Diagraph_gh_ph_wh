using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Next : MonoBehaviour
{
    public GameObject[] GA_objects;
    public GameObject[] GA_questions;
    public GameObject G_options,G_question;
    public int I_count;
    public GameObject G_final;
    public AudioSource AS_click, AS_wrg;
    // Start is called before the first frame update
    void Start()
    {
        I_count = 0;
        G_final.SetActive(false);
        showquestion();
    }
    public void showquestion()
    {
        for(int i=0;i<GA_objects.Length;i++)
        {
            GA_objects[i].SetActive(false);
            GA_questions[i].SetActive(false);
        }
        GA_objects[I_count].SetActive(true);
        G_question.SetActive(false);
        G_options.SetActive(true);
    }
    
    public void BUT_clicking()
    {
        GameObject dummy = EventSystem.current.currentSelectedGameObject;
        
        if(int.Parse(dummy.tag)==I_count+1)
        {
            Invoke("showans", 1f);
            AS_click.Play();
            
        }else
        {
            AS_wrg.Play();
        }
    }
    public void showans()
    {
        G_options.SetActive(false);
        G_question.SetActive(true);
        GA_questions[I_count].SetActive(true);
    }
    public void BUT_next()
    {
        I_count ++;
        if(I_count<GA_objects.Length)
        {
            showquestion();
        }
        else
        {
            G_final.SetActive(true);
        }
        
    }
}
