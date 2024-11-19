using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [CanBeNull] public Image itemSprite;
    public Image equippedOutline;
    public bool isEquipped = false;
    [CanBeNull] public ItemData slotItemData;
    public GameObject itemPanelGameObject;
    public Vector2Int inventoryPosition;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    
    public delegate void OnHoverSlot(InventorySlot slot);
    public static event OnHoverSlot HoverSlotEvent;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void Update()
    {
        if (slotItemData != null)
        {
            itemSprite.gameObject.SetActive(true);
            itemSprite.sprite = slotItemData.itemSprite;
        }
    }
    
    public void InitializeSlot(Vector2Int position)
    {
        inventoryPosition = position;
        
        // if (itemData != null)
        // {
        //     itemSprite.sprite = itemData.itemSprite; 
        // }
    }

    public void ClearSlot()
    {
        this.slotItemData = null;
        this.itemSprite.sprite = null;
        this.itemSprite = null;
    }

    public void SetItemData(ItemData itemData)
    {
        slotItemData = itemData;
        itemSprite.gameObject.SetActive(true);
        itemSprite.sprite = slotItemData.itemSprite;
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
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotItemData != null && itemPanelGameObject != null)
        {
            ItemPanel itemPanel = itemPanelGameObject.GetComponent<ItemPanel>();
            if (itemPanel != null)
            {
                itemPanel.BuildPanel(slotItemData);
            }
            itemPanel.gameObject.SetActive(true); // Show the panel on hover
        }
        
        HoverSlotEvent?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemPanelGameObject != null)
        {
            itemPanelGameObject.gameObject.SetActive(false); // Hide the panel when no longer hovering
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotItemData != null)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
            originalPosition = transform.position;
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
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        transform.position = originalPosition;
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop called on: " + gameObject.name);
    
        InventorySlot draggedSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
    
        if (draggedSlot != null && draggedSlot != this)
        {
            Debug.Log("Swapping items between slots.");
            
            draggedSlot.SetItemData(this.slotItemData);
            ClearSlot();
        }
        else
        {
            Debug.LogWarning("OnDrop: draggedSlot is null or invalid.");
        }
    }
    
}
