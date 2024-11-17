using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemSprite;
    public Image selectedOutline;
    public ItemData itemData;
    public GameObject itemPanelGameObject;
    
    public delegate void OnHoverSlot(InventorySlot slot);
    public static event OnHoverSlot HoverSlotEvent;

    public void SetSelectedState(bool state)
    {
        if (state)
        {
            selectedOutline.gameObject.SetActive(true);
        }

        if (!state)
        {
            selectedOutline.gameObject.SetActive(false);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null && itemPanelGameObject != null)
        {
            ItemPanel itemPanel = itemPanelGameObject.GetComponent<ItemPanel>();
            if (itemPanel != null)
            {
                itemPanel.BuildPanel(itemData);
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
    
}
