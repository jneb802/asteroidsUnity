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
    public float moveSpeed = 5f;

    public float turnSpeed = 1.0f;
    public Rigidbody2D playerRigidbody2D;
    private Vector2 _targetPosition;
    private bool _isMoving;
    
    private bool _thrusting;
    private bool _boosting;
    private bool _strafeRight;
    private bool _strafeLeft;
    private float _turnDirection;
    public InventorySlot weaponSlot1;
    public InventorySlot weaponSlot2;
    public InventorySlot weaponSlot3;
    public InventorySlot weaponSlot4;
    
    public Inventory inventory;
    private bool showFullInventoryUI;
    
    public ItemData startingItem;
    
    public GameObject playerInventoryPrefab;
    public StatBars statBars;
    
    private void Awake()
    {
        health = 100;
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        
        inventory = gameObject.AddComponent<Inventory>();
        inventory.Init(3,8);

        foreach (InventorySlot slot in playerInventoryPrefab.GetComponentsInChildren<InventorySlot>(true))
        {
            Vector2Int key = slot.inventoryPosition;
            inventory.inventorySlots.Add(key, slot);
            // Debug.Log("Added inventory slot with position: " + key);
        }
        
        inventory.AddItem(startingItem);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Convert mouse position to world position
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _targetPosition = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
            _isMoving = true; // Enable movement
        }
        
        // While holding down Mouse0, update the target position to follow the mouse
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _targetPosition = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
            _isMoving = true; // Ensure movement continues
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _isMoving = false; // Stop movement when Mouse0 is released
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TriggerItem(weaponSlot1.slotItemData);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            TriggerItem(weaponSlot2.slotItemData);
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerItem(weaponSlot3.slotItemData);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            TriggerItem(weaponSlot4.slotItemData);
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
    
    private void MoveToTarget()
    {
        Vector2 direction = (_targetPosition - playerRigidbody2D.position).normalized;
        playerRigidbody2D.velocity = direction * moveSpeed;
        if (Vector2.Distance(playerRigidbody2D.position, _targetPosition) < 0.1f)
        {
            playerRigidbody2D.velocity = Vector2.zero;
            _isMoving = false;
        }
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            MoveToTarget();
        }
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
                playerRigidbody2D.velocity = Vector2.zero;
                playerRigidbody2D.angularVelocity = 0f;
            
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
