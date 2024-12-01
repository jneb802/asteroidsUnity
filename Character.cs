using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Character : MonoBehaviour
{
    public ParticleSystem deathEffect;
    public ItemData primaryWeapon;
    public ItemData engine;
    public ItemData armor;
    public float shield;
    public float maxShield;
    public float shieldRegenRate;
    
    public float energy;
    public float maxEnergy;
    public float energyPerSecond;
    
    public GameObject lootPrefab;
    
    public SpriteRenderer gunMountLeft;
    public SpriteRenderer gunMountRight;

    public void TriggerItem(ItemData itemData)
    {
        if (itemData == null)
        {
            return;
        }
        
        if (!UseEnergy(itemData.energyCost))
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
        
        Vector3 fireDirection = (mousePosition - transform.position).normalized;
        float spawnRadius = 0.75f;
        Vector3 spawnPosition = transform.position + fireDirection * spawnRadius;
        
        Projectile projectile = Instantiate(
            itemdata.projectile, 
            spawnPosition, 
            Quaternion.LookRotation(Vector3.forward, fireDirection)
        );

        projectile.Project(fireDirection, itemdata.damageValue);
    }
    
    public void FireProjectile(ItemData itemdata, Vector3 direction)
    {
        Vector3 fireDirection = (direction - transform.position).normalized;
        
        Vector3 spawnPosition = transform.position + fireDirection * 1f;
        
        Projectile projectile = Instantiate(
            itemdata.projectile, 
            spawnPosition, 
            Quaternion.LookRotation(Vector3.forward, fireDirection)
        );

        projectile.Project(fireDirection, itemdata.damageValue);
    }
    
    public void TriggerDeath()
    {
        ParticleSystem effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        effect.Play();
            
        Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constant);
            
        DropLoot();
        
        Destroy(this.gameObject);
    }

    public void DropLoot()
    {
        if (lootPrefab == null)
        {
            return;
        }
        
        float randomNumber = Random.value;

        if (randomNumber < 0.95f)
        {
            if (lootPrefab != null)
            {
                GameObject cargoCrate = Instantiate(lootPrefab, transform.position, Quaternion.identity);
                LootDrop lootDrop = cargoCrate.GetComponent<LootDrop>();
                if (lootDrop != null)
                {
                    lootDrop.SetTrajectory(Random.insideUnitCircle.normalized);
                }
            } 
        }
    }

    public bool UseEnergy(float amount)
    {
        if (energy >= amount)
        {
            energy -= amount;
            return true;
        }
        
        return false;
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TriggerDeath();
        }
    }
}
