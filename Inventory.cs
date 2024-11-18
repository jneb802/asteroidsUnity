using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    
    public void AddItem(ItemData item, int quantity = 1)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] += quantity;
        }
        else
        {
            inventory[item] = quantity;
        }
        Debug.Log($"Added {quantity} of {item.name}. Total: {inventory[item]}");
    }
}