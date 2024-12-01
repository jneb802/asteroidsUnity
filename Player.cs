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
    public Inventory equipment;
    public Inventory utilities;
    private bool showFullInventoryUI;
    
    public ItemData startingItem;
    
    public GameObject playerInventoryPrefab;
    public GameObject playerEquipmentPrefab;
    public StatBars statBars;
    
    private void Awake()
    {
        shield = 100;
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        
        inventory = gameObject.AddComponent<Inventory>();
        inventory.Init(3,8);

        foreach (InventorySlot slot in playerInventoryPrefab.GetComponentsInChildren<InventorySlot>(true))
        {
            Vector2Int key = slot.inventoryPosition;
            inventory.cargoSlots.Add(key, slot);
        }
        
        inventory.AddItem(startingItem);
    }
    
    private void Update()
    {
        RegenerateEnergy();
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _targetPosition = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
            _isMoving = true; // Enable movement
        }
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _targetPosition = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
            _isMoving = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _isMoving = false;
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
    
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            if (shield > 0)
            {
                Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
                float damageAmount = GetAsteroidDamage(asteroid);
                Damage(damageAmount);
            }
            if (shield <= 0)
            {
                playerRigidbody2D.velocity = Vector2.zero;
                playerRigidbody2D.angularVelocity = 0f;
            
                this.gameObject.SetActive(false);
            
                GameManager.Instance.PlayerDied();
            }
        }
        
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log(collision.gameObject.name);
            if (shield > 0)
            {
                Projectile projectile = collision.gameObject.GetComponent<Projectile>();
                Damage(projectile.damageValue);
            }
            if (shield <= 0)
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
        float currentShield = shield;
        shield -= damage;
        statBars.SetValueBarSize("health", shield / maxShield);
    }

    public void RegenerateEnergy()
    {
        if (energy < maxEnergy)
        {
            energy += energyPerSecond * Time.deltaTime;
            energy = Mathf.Clamp(energy, 0, maxEnergy); // Ensure energy stays within bounds
            statBars.SetValueBarSize("mana", energy / maxEnergy); // Update the energy bar
        }
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
