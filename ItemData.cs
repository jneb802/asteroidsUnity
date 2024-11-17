using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite itemSprite;
    public Sprite shipSprite;
    
    // Weapon stats
    public int damageValue;
    
    // Weapon projectile
    public float speed = 500f;
    public float maxLife = 10.0f;
    public Sprite projectileSprite;
    public Projectile projectile;
    
    // Armor stats
    public float armorDurability;
    
    // Engine stats
    public float propulsion;
    public float rotationalDamping;
}

public enum ItemType
{
    Weapon,
    Armor,
    Engine
}


