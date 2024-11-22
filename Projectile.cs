using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public float speed = 500f;
    public float maxLife = 10.0f;
    public ProjectileType projectileType;
    public float explosionRadius = 5.0f;
    public LayerMask damageableLayer; 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        _rigidbody.AddForce(direction * speed);
        
        Destroy(this.gameObject, maxLife);
    }
    
    private void Explode()
    {
        // Find all objects within the explosion radius
        Collider2D[] objectsInRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageableLayer);

        foreach (Collider2D obj in objectsInRadius)
        {
            Destroy(obj.gameObject);
        }
        
        Debug.Log("Bomb exploded, destroying objects in radius!");
    }
    
    private void OnDrawGizmosSelected()
    {
        // Visualize the explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (projectileType == ProjectileType.Explosive)
        {
            Explode();
        }

        Destroy(this.gameObject);
    }
}

public enum ProjectileType
{
    Bullet,
    Explosive, 
    Charge,
    Boomerang,
    Missle,
}