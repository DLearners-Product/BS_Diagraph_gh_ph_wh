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
#region QA
    private int qIndex;
    public GameObject questionGO;
    public GameObject[] optionsGO;
    public bool isActivityCompleted = false;
    public Dictionary<string, Component> additionalFields;
    Component question;
    Component[] options;
    Component[] answers;
#endregion

    void Start()
    {
#region DataSetter
        // Main_Blended.OBJ_main_blended.levelno = 6;
        QAManager.instance.UpdateActivityQuestion();
        qIndex = 0;
        GetAdditionalData();
#endregion
        ChangeQues();
    }

    void ChangeQues()
    {
        if(currentIndex == questions.Length) { BlendedOperations.instance.NotifyActivityCompleted(); activityCompleted.SetActive(true); return; }

        GetData(currentIndex);
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
            ScoreManager.instance.RightAnswer(qIndex++, questionID: question.id, answerID: GetOptionID(selectedOptSTR));
            questionText.text = currentQuesOpt.answerText;
            DisableInteraction();
            StartCoroutine(PlayAnswerClipAndChangeQues(rightOption));
        }else{
            ScoreManager.instance.WrongAnswer(qIndex, questionID: question.id, answerID: GetOptionID(selectedOptSTR));
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

#region QA
    int GetOptionID(string selectedOption)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].text == selectedOption)
            {
                return options[i].id;
            }
        }
        return -1;
    }

    void GetData(int questionIndex)
    {
        question = QAManager.instance.GetQuestionAt(0, questionIndex);
        options = QAManager.instance.GetOption(0, questionIndex);
        answers = QAManager.instance.GetAnswer(0, questionIndex);
    }
 
    void GetAdditionalData()
    {
        additionalFields = QAManager.instance.GetAdditionalField(0);
    }
#endregion
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