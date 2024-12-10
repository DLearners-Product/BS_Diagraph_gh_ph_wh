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
    int currentIndex = 0;
    QuestionOptions currentQuesOpt;

    void Start()
    {
        ChangeQues();
    }

    void ChangeQues()
    {
        if(currentIndex == questions.Length) activityCompleted.SetActive(true);
        Utilities.Instance.ANIM_RotateHide(questionImage.transform.parent, ChangeSpriteAndRotate);
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
        string selectedOptSTR = clickedBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        var rightOption = GetRightOption();
        if(rightOption != null && selectedOptSTR == rightOption.option)
        {
            audioSource.PlayOneShot(rightOption.optionClip);
            Debug.Log("Right Options....");
            Invoke(nameof(ChangeQues), rightOption.optionClip.length + 1);
        }else{
            Debug.Log("Wrong Options....");
        }
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
}

[System.Serializable]
public class QuestionOptions
{
    public string question;
    public AudioClip questionClip;
    public TextOption[] options;
}

[System.Serializable]
public class TextOption
{
    public string option;
    public AudioClip optionClip;
    public bool isCorrect;
}