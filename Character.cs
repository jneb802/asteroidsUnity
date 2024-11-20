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
        
        Projectile projectile = Instantiate(
            primaryWeapon.projectile, 
            gunMountLeft.transform.position,
            gunMountLeft.transform.rotation
        );
            
        projectile.Project(gunMountLeft.transform.up);
    }
}
