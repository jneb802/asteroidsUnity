using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
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
}