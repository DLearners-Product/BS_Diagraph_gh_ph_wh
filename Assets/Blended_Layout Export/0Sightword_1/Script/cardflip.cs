using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cardflip : MonoBehaviour
{
    public int k ;
    private void Start()
    {
        k = 0;
    }
    public void BUT_cardflips()
    {
        k++;
        GameObject dummy = EventSystem.current.currentSelectedGameObject;
        Debug.Log(dummy.name);
        if (k % 2 != 0)
        {
            dummy.transform.GetChild(0).GetComponent<Animator>().Play("cardflip1");
        }
        else
        {
            dummy.transform.GetChild(0).GetComponent<Animator>().Play("reverseflip");
        }
    }
}
