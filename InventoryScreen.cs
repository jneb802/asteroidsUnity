using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScreen : Inventory
{
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
