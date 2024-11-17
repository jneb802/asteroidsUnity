using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    private Rigidbody2D _rigidbody;
    private bool _thrusting;
    private float _turnDirection;
    
    public ItemData primaryWeapon;
    public ItemData secondaryWeapon;

    public ItemData engine;
    public ItemData armor;
    public float durability;
    
    public SpriteRenderer gunMountLeft;
    public SpriteRenderer gunMountRight;
    
    public InventoryHUD inventoryHUD;
    public InventoryScreen inventoryScreen;
    private bool showInventoryScreen;
    
    private bool isRotating = false;
    public float rotationSpeed = 90f; // Speed of rotation in degrees per second
    private float targetAngle;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        SetupItems();
    }
    
    private void RotateShip(float rotation)
    {
        if (!isRotating)
        {
            targetAngle = transform.eulerAngles.z + rotation;
            isRotating = true;
        }
    
        float currentAngle = transform.eulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, 0, newAngle);
    
        if (Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle)) < 0.1f)
        {
            isRotating = false; // Stop rotation when target is reached
        }
    }
    
    private void Update()
    {
        _thrusting = Input.GetKey(KeyCode.W);
        if (Input.GetKey(KeyCode.A))
        {
            _turnDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _turnDirection = -1f;
        }
        else
        {
            _turnDirection = 0f;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && !isRotating) // Change key if needed
        {
            RotateShip(45f);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireProjectile(primaryWeapon);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(0);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeapon(1);
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // This will invert the current state of the boolean
            showInventoryScreen = !showInventoryScreen;
            
            inventoryScreen.gameObject.SetActive(showInventoryScreen);
            inventoryHUD.gameObject.SetActive(!showInventoryScreen);

            GameManager.Instance.TogglePause(showInventoryScreen);
        }
    }

    private void SetWeapon(int inventorySlot)
    {
        primaryWeapon = inventoryHUD.GetItemData(inventorySlot);
        inventoryHUD.SelectSlot(inventorySlot);
    }

    private void FixedUpdate()
    {
        if (_thrusting)
        {
            // Moving forward is always up in 2D game, whereas it's forward in 3D game
            _rigidbody.AddForce(this.transform.up);
        }

        if (_turnDirection != 0)
        {
            _rigidbody.AddTorque(_turnDirection * turnSpeed);
        }
    }
    
    private void SetupItems()
    {
        if (primaryWeapon != null)
        {
            gunMountLeft.sprite = primaryWeapon.shipSprite;    
        }
        if (secondaryWeapon != null)
        {
            gunMountRight.sprite = secondaryWeapon.shipSprite;    
        }
        
        if (engine != null)
        {
            _rigidbody.drag = engine.propulsion;
            _rigidbody.angularDrag = engine.rotationalDamping;
        }
        
        if (armor != null)
        {
            durability = armor.armorDurability;
        }
    }

    private void FireProjectile(ItemData itemdata)
    {
        Projectile projectile = Instantiate(
            primaryWeapon.projectile, 
            gunMountLeft.transform.position,
            gunMountLeft.transform.rotation
        );
            
        projectile.Project(gunMountLeft.transform.up);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            if (durability > 0)
            {
                Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
                float damageAmount = GetAsteroidDamage(asteroid);
                durability -= damageAmount;
            }
            if (durability <= 0)
            {
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.angularVelocity = 0f;
            
                this.gameObject.SetActive(false);
            
                GameManager.Instance.PlayerDied();
            }
        }
    }

    private float GetAsteroidDamage(Asteroid asteroid)
    {
        float smallThreshold = asteroid.minSize + (asteroid.maxSize - asteroid.minSize) / 3;
        float mediumThreshold = asteroid.minSize + 2 * (asteroid.maxSize - asteroid.minSize) / 3;
        
        if (asteroid.size <= smallThreshold)
        {
            return 1;
        }
        else if (asteroid.size > smallThreshold && asteroid.size <= mediumThreshold)
        {
            return 5;
        }
        else if (asteroid.size > mediumThreshold && asteroid.size <= asteroid.maxSize)
        {
            return 10;
        }
        
        return 0;
    }
}
