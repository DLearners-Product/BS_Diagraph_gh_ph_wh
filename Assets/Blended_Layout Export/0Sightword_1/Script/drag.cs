using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class drag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 mousePos;
    public Vector2 initalPos;

    bool isdrag;
    GameObject otherGameObject;
    
    Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Start()
    {
        //initalPos = this.GetComponent<RectTransform>().position;
        initalPos = this.transform.position;
    }


    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePos;
        //Debug.Log("Drag");
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
       // Debug.Log("End Drag");
        if(otherGameObject!=null)
        {
            if (this.gameObject.name == otherGameObject.name)
            {
                
                dragmain.OBJ_dragmain.CLR_Coloor= this.gameObject.GetComponent<Image>().color;
                dragmain.OBJ_dragmain.STR_Selected = this.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                dragmain.OBJ_dragmain.AC_Clip = this.gameObject.GetComponent<AudioSource>().clip;
                dragmain.OBJ_dragmain.THI_Correct();
                this.GetComponent<drag>().enabled = false;
                Destroy(this.gameObject);
            }
            else
            {
               // Nextonly.OBJ_Nextonly.THI_Wrong();
                dragmain.OBJ_dragmain.THI_wrg();
                this.transform.position = initalPos;
            }
        }else
        {
            this.transform.position = initalPos;
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.parent.name == "Drop")
            otherGameObject = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent.name == "Drop")
            otherGameObject = null;

    }

}
