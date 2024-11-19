using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public ItemData itemData;
    private Rigidbody2D _rigidbody;
    public float speed = 2.0f;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
        
    public void SetTrajectory(Vector2 direction)
    {
        _rigidbody.AddForce(direction * speed);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Inventory playerInventory = collision.gameObject.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.AddItem(itemData);
            }
            Destroy(this.gameObject);
        }
    }
}
