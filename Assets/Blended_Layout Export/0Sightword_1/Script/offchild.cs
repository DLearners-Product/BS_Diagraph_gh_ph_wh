using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offchild : MonoBehaviour
{
    public static offchild OBJ_offchild;
    public int I_value,I_index;
    public bool B_callONce, incrementonce;
    public AudioSource AS_source;
    public AudioSource AS_but;
    public AudioClip AC_clip;
    void Awake() 
    { 
        OBJ_offchild = this;
        B_callONce = true;

       for (int i=0;i<this.transform.childCount;i++)
       {
            this.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
       }
    }
    private void Update()
    {
        if (B_callONce)
        {
            Debug.Log("callonce");
            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (this.transform.GetChild(i).transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    Debug.Log("active");
                    if (incrementonce)
                    {
                        Debug.Log("increment");
                        I_index++;
                        AS_but.Play();
                        incrementonce = false;
                        if (I_index == I_value)
                        {
                            PlayAudio();
                        }
                    }
                   
                }
            }
        }
        
    }
    public void PlayAudio()
    {
        B_callONce = false;
        AS_source.clip = AC_clip;
        AS_source.Play();
        Invoke("offscript", AC_clip.length + 1f);
    }
    public void offscript()
    {
        this.GetComponent<offchild>().enabled = false;
    }
}
