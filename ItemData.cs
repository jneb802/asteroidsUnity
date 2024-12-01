using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string localizationKey;
    public string itemName;
    public ItemType itemType;
    public Sprite itemSprite;
    public Sprite shipSprite;
    
    // Shield stats
    public int shieldAmount;
    public float shieldRegenRate;
    
    // Propulsion stats
    public int thrustAmount;
    public float acellerationRate;
    
    // Cooling stats
    public int coolingCapacity;
    public float coolingRegenRate;
    
    // Weapon stats
    public int damageValue;
    public int energyCost;
    
    public float speed = 500f;
    public float maxLife = 10.0f;
    public Sprite projectileSprite;
    public Projectile projectile;
    public Movement movement;
    
}

public enum ItemType
{
    Weapon,
    Movement,
    Equipment,
    Utility
}


