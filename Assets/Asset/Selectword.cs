using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Selectword : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI TEX_tmp;
    public string[] STRA_wordsBefore, STRA_wordsAfter;
    public string STR_Selected;
    public GameObject G_Selected ,G_Yellow, G_Green;
    public AudioSource AS_Wrong;
    // Start is called before the first frame update
    void Start()
    {
        THI_seperateTMP();
        G_Yellow.GetComponent<Outline>().enabled = false;
        G_Green.GetComponent<Outline>().enabled = false;
    }

    void THI_seperateTMP()
    {
        STRA_wordsBefore = new string[0];

        while (TEX_tmp.text.Contains("<"))
        {
            int startIndex = TEX_tmp.text.IndexOf("<");
            int stringLength = TEX_tmp.text.Length - 1;
            string unwantedString = TEX_tmp.text.Substring(startIndex, stringLength - startIndex);
            int endIndex = unwantedString.IndexOf(">") + 1;
            unwantedString = TEX_tmp.text.Substring(startIndex, endIndex);
            TEX_tmp.text = TEX_tmp.text.Replace(unwantedString.Trim(), "");
           // Debug.Log("CLICK TEXT REDEFINED before : " + TEX_tmp.text);
        }

        STRA_wordsBefore = TEX_tmp.text.ToString().Split(' ');
        STRA_wordsAfter = new string[0];


        STRA_wordsAfter = TEX_tmp.text.ToString().Split(' ');
       // Main_Blended.OBJ_main_blended.STRA_beforeWords = STRA_wordsBefore;


        for (int i = 0; i < STRA_wordsAfter.Length; i++)
        {
            STRA_wordsAfter[i] = "<link =" + STRA_wordsAfter[i] + ">" + STRA_wordsAfter[i] + "</link>";
        }
        TEX_tmp.text = string.Join(" ", STRA_wordsAfter);

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        STR_Selected= TEX_tmp.textInfo.linkInfo[TMP_TextUtilities.FindIntersectingLink(TEX_tmp, Input.mousePosition, Camera.main)].GetLinkText();
        for (int i = 0; i < STRA_wordsAfter.Length; i++)
        {
            STRA_wordsAfter[i] = "<link =" + STRA_wordsBefore[i] + ">" + STRA_wordsBefore[i] + "</link>";
            if (STRA_wordsBefore[i] == STR_Selected)
            {
                if(G_Selected.name=="Green")
                {
                    if (STR_Selected == "sick") { STRA_wordsAfter[i] = "<link =" + STRA_wordsBefore[i] + "><mark=#1BFF0044><u>" + STRA_wordsBefore[i] + "</mark></u></link>"; }
                    if (STR_Selected == "tricks.") { STRA_wordsAfter[i] = "<link =" + STRA_wordsBefore[i] + "><mark=#1BFF0044><u>" + STRA_wordsBefore[i] + "</mark></u></link>"; }
                    if (STR_Selected == "stick") { STRA_wordsAfter[i] = "<link =" + STRA_wordsBefore[i] + "><mark=#1BFF0044><u>" + STRA_wordsBefore[i] + "</mark></u></link>"; }
                    if (STR_Selected == "chick.") { STRA_wordsAfter[i] = "<link =" + STRA_wordsBefore[i] + "><mark=#1BFF0044><u>" + STRA_wordsBefore[i] + "</mark></u></link>"; }
                    if (STR_Selected == "black") { STRA_wordsAfter[i] = "<link =" + STRA_wordsBefore[i] + "><mark=#1BFF0044><u>" + STRA_wordsBefore[i] + "</mark></u></link>"; }
                    if (STR_Selected == "click") { STRA_wordsAfter[i] = "<link =" + STRA_wordsBefore[i] + "><mark=#1BFF0044><u>" + STRA_wordsBefore[i] + "</mark></u></link>"; }
                }
                else
                if (G_Selected.name == "Yellow")
                {
                    if (STR_Selected == "pink") { STRA_wordsAfter[i] = "<link =" + STRA_wordsBefore[i] + "><mark=#ffff0044><u>" + STRA_wordsBefore[i] + "</mark></u></link>"; }
                    if (STR_Selected == "book.") { STRA_wordsAfter[i] = "<link =" + STRA_wordsBefore[i] + "><mark=#ffff0044><u>" + STRA_wordsBefore[i] + "</mark></u></link>"; }
                    if (STR_Selected == "look") { STRA_wordsAfter[i] = "<link =" + STRA_wordsBefore[i] + "><mark=#ffff0044><u>" + STRA_wordsBefore[i] + "</mark></u></link>"; }
                    if (STR_Selected == "masks.") { STRA_wordsAfter[i] = "<link =" + STRA_wordsBefore[i] + "><mark=#ffff0044><u>" + STRA_wordsBefore[i] + "</mark></u></link>"; }
                }
                else
                {
                    AS_Wrong.Play();
                }    
            }
           
        }
        TEX_tmp.text = string.Join(" ", STRA_wordsAfter);
        //Debug.Log(TEX_tmp.text);
    }

    public void BUT_SelectColor()
    {
        G_Selected = EventSystem.current.currentSelectedGameObject;
        G_Yellow.GetComponent<Outline>().enabled = false;
        G_Green.GetComponent<Outline>().enabled = false;
        G_Selected.GetComponent<Outline>().enabled = true;


    }
}
