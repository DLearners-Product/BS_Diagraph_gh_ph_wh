using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Thumbnail10Controller : MonoBehaviour, IPointerClickHandler
{
    public AudioSource AS_emptyAudioSource;
    public AudioClip AC_passageClip;
    public TextMeshProUGUI passageTMPPro;
    string[] answerContainStrings = {"gh", "ph", "wh"};
    public AudioClip[] rightAnswerClips;

    void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            var tmPRO = passageTMPPro;
            var wordIndex = TMP_TextUtilities.FindIntersectingWord(tmPRO, Input.mousePosition, Camera.main);

            if (wordIndex != -1)
            {
                var clickedWord = tmPRO.textInfo.wordInfo[wordIndex].GetWord();
                if(EvaluateAnswer(clickedWord))
                {
                    passageTMPPro.text = HighLightAnswer(tmPRO.text, wordIndex);
                    AS_emptyAudioSource.PlayOneShot(GetAudioClip(clickedWord));
                    Debug.Log("Clicked right word " + clickedWord);
                }else{
                    Debug.Log("Clicked wrong word " + clickedWord);
                }

            }
        }
    }

    AudioClip GetAudioClip(string searchText)
    {
        foreach (var voClip in rightAnswerClips)
        {
            if(voClip.name.ToLower().Contains(searchText.ToLower()))
            {
                Debug.Log($"Search Text {searchText} Name :: {voClip.name} ");
                return voClip;
            }
        }
        return null;
    }

    bool EvaluateAnswer(string passageSTR)
    {
        foreach (var answerContainStr in answerContainStrings)
        {
            if (passageSTR.ToLower().Contains(answerContainStr))
            {
                return true;
            }
        }
        return false;
    }

    string HighLightAnswer(string passageSTR, int ansIndex)
    {
        string[] passStrArr = passageSTR.Split(' ');

        if(!passStrArr[ansIndex].Contains("color=yellow"))
            passStrArr[ansIndex] = $"<color=yellow>{passStrArr[ansIndex]}</color>";

        return String.Join(" ", passStrArr);
    }
}
