using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [CanBeNull] public ItemData slotItemData = null;
    public Image itemSprite;
    public Image equippedOutline;
    public bool isEquipped = false;
    public GameObject itemPanelGameObject;
    public Vector2Int inventoryPosition;
    private CanvasGroup _canvasGroup; 
    private Vector2 _originalPosition;
    
    // public delegate void OnHoverSlot(InventorySlot slot);
    // public static event OnHoverSlot HoverSlotEvent;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void UpdateSlot()
    {
        if (slotItemData != null)
        {
            itemSprite.gameObject.SetActive(true);
            itemSprite.sprite = slotItemData.itemSprite;
        }
        else
        {
            this.itemSprite.gameObject.SetActive(false);
            this.itemSprite.sprite = null;
            this.SetEquippedState(false);
        }
    }

    public void ClearSlot()
    {
        slotItemData = null;
    }

    public void SetItemData(ItemData itemData)
    {
        this.slotItemData = itemData;
    }

    public void SetEquippedState(bool state)
    {
        if (state)
        {
            isEquipped = true;
            equippedOutline.gameObject.SetActive(true);
        }

        if (!state)
        {
            isEquipped = false;
            equippedOutline.gameObject.SetActive(false);
        }
    }
    
    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     if (slotItemData != null && itemPanelGameObject != null)
    //     {
    //         ItemPanel itemPanel = itemPanelGameObject.GetComponent<ItemPanel>();
    //         if (itemPanel != null)
    //         {
    //             itemPanel.BuildPanel(slotItemData);
    //         }
    //         itemPanel.gameObject.SetActive(true); // Show the panel on hover
    //     }
    //     
    //     HoverSlotEvent?.Invoke(this);
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     if (itemPanelGameObject != null)
    //     {
    //         itemPanelGameObject.gameObject.SetActive(false); // Hide the panel when no longer hovering
    //     }
    // }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag called from slot with position: " + inventoryPosition);
        if (slotItemData != null)
        {
            _canvasGroup.alpha = 0.6f;
            _canvasGroup.blocksRaycasts = false;
            _originalPosition = transform.position;
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (slotItemData != null)
        {
            transform.position = eventData.position;
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        transform.position = _originalPosition;
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop called on: " + gameObject.name);
    
        InventorySlot originalSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        
        if (originalSlot != null && originalSlot != this)
        {
            Debug.Log("Original slot is position: " + originalSlot.inventoryPosition);
            Debug.Log("This slot is position: " + this.inventoryPosition);
            Debug.Log("Swapping items between slots.");
            
            SetItemData(originalSlot.slotItemData);
            originalSlot.ClearSlot();
            originalSlot.UpdateSlot();
            this.UpdateSlot();
        }
        else
        {
            Debug.LogWarning("OnDrop: draggedSlot is null or invalid.");
        }
    }
    
}
