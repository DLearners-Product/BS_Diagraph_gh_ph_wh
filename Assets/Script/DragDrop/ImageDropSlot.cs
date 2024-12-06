using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ImageDropSlot: MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    ImageDragandDrop dragitem;
    public delegate void OnDropInSlotDelegate(GameObject dropedObject, GameObject dropSlotObj);
    public static OnDropInSlotDelegate onDropInSlot;

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
        // Debug.Log($"POINTER ENTERED FOR {gameObject.name}");
    }
}

