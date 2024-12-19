﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
// using UnityEngine.UIElements;
using UnityEngine.UI;

public class Thumbnail10Controller : MonoBehaviour, IPointerClickHandler
{
    public AudioSource AS_emptyAudioSource;
    public AudioClip AC_passageClip;
    public TextMeshProUGUI passageTMPPro;
    string[] answerContainStrings = {"gh", "ph", "wh"};
    public AudioClip[] rightAnswerClips;
    public GameObject nextBtn;
    public GameObject finalScreen;
    public Animator storyAnimator;
    int totalyAnswered = 0;
    int allCrctAnsCount = 0;
    List<int> answeredIndexes = new List<int>();

    void Start()
    {
        nextBtn.GetComponent<Button>().interactable = false;
        allCrctAnsCount = GetAllCrctAns();
    }

    int GetAllCrctAns()
    {
        int crctAns = 0;
        string passageText = passageTMPPro.text;
        string[] passageTextArr = passageText.Split(' ');
        for (int i = 0; i < passageTextArr.Length; i++)
        {
            if(EvaluateAnswer(passageTextArr[i]))
            {
                crctAns++;
            }
        }
        return crctAns;
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
                    if(!answeredIndexes.Contains(wordIndex))
                    {
                        ++totalyAnswered;
                        answeredIndexes.Add(wordIndex);
                    }

                    passageTMPPro.text = HighLightAnswer(tmPRO.text, wordIndex);
                    AS_emptyAudioSource.PlayOneShot(GetAudioClip(clickedWord));
                    Debug.Log("Clicked right word " + clickedWord);
                }else{
                    Debug.Log("Clicked wrong word " + clickedWord);
                }
            }

            if(totalyAnswered == allCrctAnsCount) {
                Debug.Log("BUTTON ACTTIVATED.......");
                nextBtn.GetComponent<Button>().interactable = true;
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

    public void ActivityCompleted()
    {
        finalScreen.SetActive(true);
        storyAnimator.Play("New State");
    }
}
