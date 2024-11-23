using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Character : MonoBehaviour
{
    public ItemData primaryWeapon;
    public ItemData engine;
    public ItemData armor;
    public float health;
    public float maxHealth;
    
    public SpriteRenderer gunMountLeft;
    public SpriteRenderer gunMountRight;

    public void TriggerItem(ItemData itemData)
    {
        if (itemData == null)
        {
            return;
        }

        switch (itemData.itemType)
        {
            case ItemType.Movement:
                HandleMovement(itemData);
                break;
            case ItemType.Weapon:
                FireProjectile(itemData);
                break;
            default:
                return;
        }
    }
    
    public void HandleMovement(ItemData itemData)
    {
        itemData.movement.Blink();
    }
    
    public void FireProjectile(ItemData itemdata)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 
        
        Vector3 fireDirection = (mousePosition - gunMountLeft.transform.position).normalized;
        
        Projectile projectile = Instantiate(
            itemdata.projectile, 
            gunMountLeft.transform.position, 
            Quaternion.LookRotation(Vector3.forward, fireDirection)
        );

        projectile.Project(fireDirection);
    }

}
