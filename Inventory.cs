using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // public Dictionary<Vector2Int, ItemData> inventoryItems = new Dictionary<Vector2Int, ItemData>();
    public Dictionary<Vector2Int, InventorySlot> cargoSlots = new Dictionary<Vector2Int, InventorySlot>();
    public Dictionary<Vector2Int, InventorySlot> equipmentSlots = new Dictionary<Vector2Int, InventorySlot>();
    public Dictionary<Vector2Int, InventorySlot> utilitySlots = new Dictionary<Vector2Int, InventorySlot>();
    public int inventoryColumns;
    public int inventoryRows;
    private Vector2Int currentSelectedWeapon;
    
    public void Init(int rows, int columns)
    {
        inventoryRows = rows;
        inventoryColumns = columns;
    }

    public ItemData EquipItem(Vector2Int position)
    {
        if (cargoSlots.ContainsKey(position))
        {
            ItemData item = cargoSlots[position].slotItemData;

            if (currentSelectedWeapon != position)
            {
                SetSlotEquipState(currentSelectedWeapon, false);
            }
            
            SetSlotEquipState(position,true);
            currentSelectedWeapon = position;
            return item;
        }
        else
        {
            Debug.LogWarning($"No item found at position {position}.");
            return null;
        }
    }
    
    public ItemData GetItemData(Vector2Int position)
    {
        if (cargoSlots.ContainsKey(position))
        {
            return cargoSlots[position].slotItemData;
        }
        else
        {
            Debug.LogWarning($"No item found at position {position}.");
            return null;
        }
    }
    
    public void AddItem(ItemData item, int quantity = 1)
    {
        Vector2Int? openPosition = FindOpenPosition();
        if (openPosition.HasValue)
        {
            cargoSlots[openPosition.Value].slotItemData = item;
            cargoSlots[openPosition.Value].UpdateSlot();
            // Debug.Log($"Added {item.name} at position {openPosition.Value}.");
        }
    }
    
    public Vector2Int? FindOpenPosition()
    {
        // Debug.Log($"Finding open position.");
        for (int y = 0; y < inventoryRows; y++)
        {
            // Debug.Log($"Checking row {y} of {inventoryRows - 1}.");
            for (int x = 0; x < inventoryColumns; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (cargoSlots.ContainsKey(position) && cargoSlots[position].slotItemData == null)
                {
                    // Debug.Log($"Found open slot at {position}.");
                    return position;
                }
            }
        }

        // Debug.LogWarning("No open positions available in the inventory.");
        return null;
    }
    
    public void SetSlotEquipState(Vector2Int slot, bool state)
    {
        if (cargoSlots.ContainsKey(slot))
        {
            if (state)
            {
                cargoSlots[slot].SetEquippedState(true);                
            }
            else
            {
                cargoSlots[slot].SetEquippedState(false);
            }
            
        }
        
    }
    
    public void ShowFullInventory()
    {
        for (int row = 1; row < inventoryRows; row++) // Start from the second row (index 1)
        {
            for (int column = 0; column < inventoryColumns; column++)
            {
                Vector2Int position = new Vector2Int(row, column);
                if (cargoSlots.ContainsKey(position))
                {
                    cargoSlots[position].gameObject.SetActive(true);
                }
            }
        }
    }
    
    public void HideFullInventory()
    {
        for (int row = 1; row < inventoryRows; row++) // Start from the second row (index 1)
        {
            for (int column = 0; column < inventoryColumns; column++)
            {
                Vector2Int position = new Vector2Int(row, column);
                if (cargoSlots.ContainsKey(position))
                {
                    cargoSlots[position].gameObject.SetActive(false);
                }
            }
        }
    }
    
}