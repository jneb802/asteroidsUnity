using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Character
{
    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    private Rigidbody2D _rigidbody;
    private bool _thrusting;
    private bool _boosting;
    private bool _strafeRight;
    private bool _strafeLeft;
    private float _turnDirection;
    
    public Inventory inventory;
    private bool showFullInventoryUI;
    
    public ItemData startingItem;
    
    public GameObject playerInventory;
    public StatBars statBars;
    
    private void Awake()
    {
        health = 100;
        _rigidbody = GetComponent<Rigidbody2D>();
        
        inventory = gameObject.AddComponent<Inventory>();
        inventory.Init(3,8);

        foreach (InventorySlot slot in playerInventory.GetComponentsInChildren<InventorySlot>(true))
        {
            Vector2Int key = slot.inventoryPosition;
            inventory.inventorySlots.Add(key, slot);
            Debug.Log("Added inventory slot with position: " + key);
        }
        
        inventory.AddItem(startingItem);
    }
    
    private void Update()
    {
        _thrusting = Input.GetKey(KeyCode.W);
        _boosting = Input.GetKey(KeyCode.LeftShift);
        if (Input.GetKey(KeyCode.A))
        {
            _turnDirection = 1f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _strafeLeft = true;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _turnDirection = -1f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _strafeRight = true;
            }
        }
        else
        {
            _turnDirection = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireProjectile(primaryWeapon);
        }
        
        for (int i = 1; i <= 8; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                SetWeapon(0,i - 1);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // This will invert the current state of the boolean
            showFullInventoryUI = !showFullInventoryUI;
            GameManager.Instance.TogglePause(showFullInventoryUI);
        }

        if (showFullInventoryUI)
        {
            inventory.ShowFullInventory();
        }
        else
        {
            inventory.HideFullInventory();
        }
    }

    private void SetWeapon(int inventoryRow, int inventoryColumn)
    {
        ItemData selectedItem = inventory.EquipItem(new Vector2Int(inventoryRow, inventoryColumn));
        if (selectedItem != null)
        {
            primaryWeapon = selectedItem;
            gunMountLeft.sprite = selectedItem.shipSprite;  
        }
        else
        {
            primaryWeapon = null;
            gunMountLeft.sprite = null;
        }
    }

    private void FixedUpdate()
    {
        if (_thrusting)
        {
            // Moving forward is always up in 2D game, whereas it's forward in 3D game
            _rigidbody.AddForce(this.transform.up);
        }
        
        if (_boosting)
        {
            _rigidbody.AddForce(this.transform.up * 2);
        }

        if (_turnDirection != 0)
        {
            _rigidbody.AddTorque(_turnDirection * turnSpeed);
        }

        if ((_strafeRight))
        {
            PerformDodge(Vector2.right);
            _strafeRight = false;
        }
        
        if ((_strafeLeft))
        {
            PerformDodge(Vector2.left);
            _strafeLeft = false;
        }
    }
    
    private void PerformDodge(Vector2 direction)
    {
        float dodgeForce = 2f; // Adjust for desired dodge intensity

        // Rotate the direction vector to align with the player's current orientation
        Vector2 dodgeDirection = (_rigidbody.transform.rotation * direction);
    
        // Apply an impulse force for the dodge
        _rigidbody.AddForce(dodgeDirection * dodgeForce, ForceMode2D.Impulse);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            if (health > 0)
            {
                Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
                float damageAmount = GetAsteroidDamage(asteroid);
                Damage(damageAmount);
            }
            if (health <= 0)
            {
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.angularVelocity = 0f;
            
                this.gameObject.SetActive(false);
            
                GameManager.Instance.PlayerDied();
            }
        }
    }

    public void Damage(float damage)
    {
        float currentHealth = health;
        health -= damage;
        statBars.SetValueBarSize("health", health / maxHealth);
    }

    private float GetAsteroidDamage(Asteroid asteroid)
    {
        float smallThreshold = asteroid.minSize + (asteroid.maxSize - asteroid.minSize) / 3;
        float mediumThreshold = asteroid.minSize + 2 * (asteroid.maxSize - asteroid.minSize) / 3;
        
        if (asteroid.size <= smallThreshold)
        {
            return 5;
        }
        else if (asteroid.size > smallThreshold && asteroid.size <= mediumThreshold)
        {
            return 10;
        }
        else if (asteroid.size > mediumThreshold && asteroid.size <= asteroid.maxSize)
        {
            return 25;
        }
        
        return 0;
    }
}
