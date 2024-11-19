using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<Vector2Int, ItemData> inventoryItems = new Dictionary<Vector2Int, ItemData>();
    public Dictionary<Vector2Int, InventorySlot> inventorySlots = new Dictionary<Vector2Int, InventorySlot>();
    public int inventoryColumns;
    public int inventoryRows;
    public int inventorySize;
    
    public void Init(int rows, int columns)
    {
        inventoryRows = rows;
        inventoryColumns = columns;
        
        inventorySize = rows * columns;
        
        if (inventorySize > 0)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    Vector2Int position = new Vector2Int(row, column);
                    
                    if (!inventorySlots.ContainsKey(position))
                    {
                        // InventorySlot newSlot = new InventorySlot(); // Or instantiate if necessary
                        // inventorySlots[position] = newSlot;
                        // newSlot.InitializeSlot(position);
                    }
                }
            }
        }
    }
    
    public ItemData GetItemData(Vector2Int position)
    {
        if (inventoryItems.ContainsKey(position))
        {
            return inventoryItems[position];
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
            inventoryItems[openPosition.Value] = item;
            Debug.Log($"Added {item.name} at position {openPosition.Value}.");
        }
    }
    
    public Vector2Int? FindOpenPosition()
    {
        for (int y = 0; y < inventoryRows; y++)
        {
            for (int x = 0; x < inventoryColumns; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (!inventoryItems.ContainsKey(position))
                {
                    return position;
                }
            }
        }

        Debug.LogWarning("No open positions available in the inventory.");
        return null;
    }
    
    public void SetSlotEquipState(Vector2Int slot)
    {
        if (inventoryItems.ContainsKey(slot))
        {
            if (inventorySlots[slot].isEquipped == false)
            {
                inventorySlots[slot].SetEquippedState(true);                
            }
            else
            {
                inventorySlots[slot].SetEquippedState(false);
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
                if (inventorySlots.ContainsKey(position))
                {
                    inventorySlots[position].gameObject.SetActive(true);
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
                if (inventorySlots.ContainsKey(position))
                {
                    inventorySlots[position].gameObject.SetActive(false);
                }
            }
        }
    }
    
}