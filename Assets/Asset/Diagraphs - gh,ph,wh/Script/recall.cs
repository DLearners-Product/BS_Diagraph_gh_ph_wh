using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class recall : MonoBehaviour
{
    public Sprite[] SPR_Pics;
    public Image IMG_Main;
    public Text TXT_Max, TXT_Current;
    public int I_Qcount;
    public GameObject G_Final;
    public Transform _spawnPoint;
    public Transform _standPoint;
    public Transform _endPoint;
    public GameObject prefabObj;
    public Transform spawnParent;
    List<GameObject> _spawnedObjects = new List<GameObject>();
    GameObject _currentSpawnQues,
                _prevSpawnQues;
    QuestionChangeOrder _movementdir;

    enum QuestionChangeOrder
    {
        Next,
        Back
    }

    void Start()
    {
        I_Qcount = 0;
        THI_ShowQuestion();
        G_Final.SetActive(false);
        TXT_Max.text = SPR_Pics.Length.ToString();
        // UpdateCounter();
    }

    void THI_ShowQuestion()
    {
        // IMG_Main.sprite = SPR_Pics[I_Qcount];
        // IMG_Main.preserveAspect = true;

        var spawnedPic = Instantiate(prefabObj, spawnParent);
        spawnedPic.GetComponent<Image>().sprite = SPR_Pics[I_Qcount];
        spawnedPic.GetComponent<Image>().preserveAspect = true;
        _prevSpawnQues = _currentSpawnQues;
        _currentSpawnQues = spawnedPic;

        if(_movementdir == QuestionChangeOrder.Next)
        {
            spawnedPic.transform.position = _spawnPoint.transform.position;
            Utilities.Instance.ANIM_MoveWithScaleUp(_currentSpawnQues.transform, _standPoint.transform.position);
            if(_prevSpawnQues != null)
            {
                Utilities.Instance.ANIM_MoveWithScaleDown(_prevSpawnQues.transform, _endPoint.transform.position, DeleteCompletedObj);
            }
        }else{
            spawnedPic.transform.position = _endPoint.transform.position;

            Utilities.Instance.ANIM_MoveWithScaleUp(_currentSpawnQues.transform, _standPoint.transform.position);
            if(_prevSpawnQues != null)
            {
                Utilities.Instance.ANIM_MoveWithScaleDown(_prevSpawnQues.transform, _spawnPoint.transform.position, DeleteCompletedObj);
            }
        }
        // _spawnedObjects.Add(spawnedPic);
    }
    // chin, ship, cherry, shell, thorn, wash, push, sheep, bath, watch

    public void BUT_Next()
    {
        _movementdir = QuestionChangeOrder.Next;

        if(I_Qcount<SPR_Pics.Length-1)
        {
            I_Qcount++;
            // UpdateCounter();
            THI_ShowQuestion();
        }
        else
        {
            G_Final.SetActive(true);
        }
    }

    public void BUT_Back()
    {
        _movementdir = QuestionChangeOrder.Back;

        if (I_Qcount > 0)
        {
            I_Qcount--;
            // UpdateCounter();
            THI_ShowQuestion();
        }
        else
        {
            G_Final.SetActive(true);
        }
    }

    void DeleteCompletedObj()
    {
        Destroy(_prevSpawnQues);
        // _spawnedObjects.RemoveAt(0);
    }

    // void UpdateCounter()
    // {
    //     int x = I_Qcount + 1;
    //     TXT_Current.text = x.ToString();
    // }
}
