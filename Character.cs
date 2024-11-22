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
    
    public void FireProjectile(ItemData itemdata)
    {
        if (itemdata == null)
        {
            return;
        }

        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure z-coordinate is 0 for 2D game

        // Calculate direction from gun mou nt to mouse position
        Vector3 fireDirection = (mousePosition - gunMountLeft.transform.position).normalized;

        // Create and fire the projectile
        Projectile projectile = Instantiate(
            itemdata.projectile, 
            gunMountLeft.transform.position, 
            Quaternion.LookRotation(Vector3.forward, fireDirection)
        );

        projectile.Project(fireDirection);
    }

}
