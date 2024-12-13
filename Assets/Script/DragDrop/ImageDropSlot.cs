using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ImageDropSlot: MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    ImageDragandDrop dragitem;
    public delegate void OnDropInSlotDelegate(GameObject dropedObject, GameObject dropSlotObj);
    public delegate void MouseEvent(GameObject dragObject, GameObject hoverObject);
    public static OnDropInSlotDelegate onDropInSlot;
    public static MouseEvent onMouseHoverEnter;
    public static MouseEvent onMouseHoverExit;

    public void OnDrop(PointerEventData eventData)
    {
        dragitem = eventData.pointerDrag.GetComponent<ImageDragandDrop>();
        onDropInSlot?.Invoke(eventData.pointerDrag, gameObject);
    }

    public void SetDropedObject(){
        dragitem.transform.SetParent(this.transform);
        dragitem.canvasGroup.alpha = 1f;
        dragitem.transform.localPosition = dragitem.currentPos;
        dragitem.enabled = false;
    }

    public void ResetDropedObjectPosition(){
        dragitem.ReturnToOriginalPos();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseHoverEnter?.Invoke(eventData.pointerDrag, gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseHoverExit?.Invoke(eventData.pointerDrag, gameObject);
    }
}

