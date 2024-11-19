// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class InventoryUI : MonoBehaviour
// {
//     public Inventory inventory;
//     public List<InventorySlot> inventorySlots;
//
//     // public void Start()
//     // {
//     //     int rows = inventory.inventoryRows;
//     //     int columns = inventory.inventoryColumns;
//     //     
//     //     if (inventory.inventorySize > 0)
//     //     {
//     //         for (int row = 0; row < rows; row++)
//     //         {
//     //             for (int column = 0; column < columns; column++)
//     //             {
//     //                 int index = row * columns + column;
//     //                 if (index < inventory.inventorySize)
//     //                 {
//     //                     inventorySlots[index].InitializeSlot(new Vector2Int(row, column));
//     //                 }
//     //             }
//     //         }
//     //     }
//     // }
//     
//     // public void Update()
//     // {
//     //     int rows = inventory.inventoryRows;
//     //     int columns = inventory.inventoryColumns;
//     //
//     //     for (int row = 0; row < rows; row++)
//     //     {
//     //         for (int column = 0; column < columns; column++)
//     //         {
//     //             Vector2Int slotPosition = new Vector2Int(row, column);
//     //             int index = row * columns + column;
//     //
//     //             if (index < inventory.inventorySize)
//     //             {
//     //                 if (inventory.inventoryItems.TryGetValue(slotPosition, out ItemData itemData))
//     //                 {
//     //                     inventorySlots[index].SetItemData(itemData);
//     //                 }
//     //                 else
//     //                 {
//     //                     inventorySlots[index].ClearSlot();
//     //                 }
//     //             }
//     //         }
//     //     }
//     // }
//     
//     // public void SelectSlot(int slot)
//     // {
//     //     ClearSelections();
//     //     inventorySlots[slot].SetSelectedState(true);
//     // }
//     //
//     // public void ClearSelections()
//     // {
//     //     foreach (var slot in inventorySlots)
//     //     {
//     //         slot.SetSelectedState(false);
//     //     }
//     // }
//     //
//     // public void ShowFullInventory()
//     // {
//     //     for (int i = 8; i < inventorySlots.Count; i++)
//     //     {
//     //         inventorySlots[i].gameObject.SetActive(true);
//     //     }
//     // }
//     //
//     // public void HideFullInventory()
//     // {
//     //     for (int i = 8; i < inventorySlots.Count; i++)
//     //     {
//     //         inventorySlots[i].gameObject.SetActive(false);
//     //     }
//     // }
//     
//     
//     private void OnEnable()
//     {
//         InventorySlot.HoverSlotEvent += OnInventorySlotHovered;
//     }
//
//     private void OnDisable()
//     {
//         InventorySlot.HoverSlotEvent -= OnInventorySlotHovered;
//     }
//
//     private void OnInventorySlotHovered(InventorySlot slot)
//     {
//         // Handle logic when a slot is hovered over
//         // Debug.Log("Hovered over slot: " + slot.name);
//
//         // Optionally, show item details in a UI panel or update the inventory HUD
//     }
// }
