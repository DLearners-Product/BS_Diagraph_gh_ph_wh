using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Thumbnail6Controller : MonoBehaviour
{
    public Image questionImage;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI option1Text,
                            option2Text;
    public Sprite[] quesSprites;
    public QuestionOptions[] questions;
    public GameObject activityCompleted;
    public AudioSource audioSource;
    public AudioClip rightAnsClip;
    public AudioClip wrongAnsClip;
    public TextMeshProUGUI counterText;
    int currentIndex = 0;
    QuestionOptions currentQuesOpt;
    bool B_interactable = true;

    void Start()
    {
        ChangeQues();
    }

    void ChangeQues()
    {
        if(currentIndex == questions.Length) { activityCompleted.SetActive(true); return; }
        Utilities.Instance.ANIM_RotateHide(questionImage.transform.parent, ChangeSpriteAndRotate);
        UpdateCounterText();
    }

    void UpdateCounterText()
    {
        counterText.text = $"{currentIndex + 1} / {questions.Length}";
    }

    void ChangeSpriteAndRotate()
    {
        currentQuesOpt = questions[currentIndex];
        questionImage.sprite = quesSprites[currentIndex];
        Utilities.Instance.ANIM_RotateShow(questionImage.transform.parent, AssignTextQuesOpt);
        currentIndex++;
    }

    void AssignTextQuesOpt()
    {
        audioSource.PlayOneShot(currentQuesOpt.questionClip);
        Invoke(nameof(EnableInteraction), currentQuesOpt.questionClip.length);
        questionText.text = currentQuesOpt.question;
        option1Text.text = currentQuesOpt.options[0].option;
        option2Text.text = currentQuesOpt.options[1].option;
    }

    public void OnQuestionPanelClicked()
    {
        audioSource.PlayOneShot(currentQuesOpt.questionClip);
    }

    public void OptionBtnClicked(GameObject clickedBtn)
    {
        if(!B_interactable) return;

        string selectedOptSTR = clickedBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        var rightOption = GetRightOption();
        if(rightOption != null && selectedOptSTR == rightOption.option)
        {
            questionText.text = currentQuesOpt.answerText;
            DisableInteraction();
            StartCoroutine(PlayAnswerClipAndChangeQues(rightOption));
        }else{
            audioSource.PlayOneShot(wrongAnsClip);
        }
    }

    IEnumerator PlayAnswerClipAndChangeQues(TextOption rightOption)
    {
        audioSource.PlayOneShot(rightAnsClip);
        yield return new WaitForSeconds(rightAnsClip.length);
        audioSource.PlayOneShot(rightOption.optionClip);
        Invoke(nameof(ChangeQues), rightOption.optionClip.length + 1);
    }

    TextOption GetRightOption()
    {
        foreach (var option in currentQuesOpt.options)
        {
            if(option.isCorrect)
                return option;
        }
        return null;
    }

    void EnableInteraction() => B_interactable = true;
    void DisableInteraction() => B_interactable = false;
}

[System.Serializable]
public class QuestionOptions
{
    public string question;
    public AudioClip questionClip;
    public string answerText;
    public TextOption[] options;
}

[System.Serializable]
public class TextOption
{
    public string option;
    public AudioClip optionClip;
    public bool isCorrect;
}