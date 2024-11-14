using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_main : MonoBehaviour
{
    public GameObject G_allcards;
    public GameObject G_draganddrop;
    int i = 0;
    public int j;
    // Start is called before the first frame update
    void Start()
    {
        j = 0;
        G_draganddrop.SetActive(false);
    }

    // Update is called once per frame
    public void BUT_next()
    {
        i++;
        if(i%2 != 0)
        {
            G_allcards.SetActive(false);
            G_draganddrop.SetActive(true);
            j++;
            if(j>1)
            {
                Initialpos.OBJ_initialpos.Reset();
            }
        }
        else
        {
            G_allcards.SetActive(true);
            for(int i=0;i<G_allcards.transform.childCount;i++)
            {
                G_allcards.transform.GetChild(i).transform.GetChild(0).GetComponent<Animator>().Play("reverseflip");
            }
            G_draganddrop.SetActive(false);
        }

    }
}
