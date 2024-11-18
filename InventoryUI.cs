using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    
    public List<InventorySlot> inventorySlots;

    public ItemData GetItemData(int slot)
    {
        return inventorySlots[slot].itemData;
    }
    
    public Image GetItemSprite(int slot)
    {
        return inventorySlots[slot].itemSprite;
    }

    public void SelectSlot(int slot)
    {
        ClearSelections();
        inventorySlots[slot].SetSelectedState(true);
    }

    public void ClearSelections()
    {
        foreach (var slot in inventorySlots)
        {
            slot.SetSelectedState(false);
        }
    }
    
    public void ShowFullInventory()
    {
        for (int i = 4; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].gameObject.SetActive(true);
        }
    }
    
    public void HideFullInventory()
    {
        for (int i = 4; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].gameObject.SetActive(false);
        }
    }
    
    
    private void OnEnable()
    {
        InventorySlot.HoverSlotEvent += OnInventorySlotHovered;
    }

    private void OnDisable()
    {
        InventorySlot.HoverSlotEvent -= OnInventorySlotHovered;
    }

    private void OnInventorySlotHovered(InventorySlot slot)
    {
        // Handle logic when a slot is hovered over
        Debug.Log("Hovered over slot: " + slot.name);

        // Optionally, show item details in a UI panel or update the inventory HUD
    }
}
