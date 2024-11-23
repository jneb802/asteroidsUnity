using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public float speed = 500f;
    public float maxLife = 10.0f;
    public ProjectileType projectileType;
    public float explosionRadius = 1.0f;
    public LayerMask damageableLayer;
    
    public float beamRange = 5f;
    public float beamWidth = 0.2f;
    public LineRenderer beamLineRenderer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        if (projectileType == ProjectileType.Beam)
        {
            FireBeam(direction);
        }
        else
        {
            _rigidbody.AddForce(direction * speed);
            Destroy(this.gameObject, maxLife);
        }
    }
    
    private void FireBeam(Vector2 direction)
    {
        Vector2 beamStart = (Vector2)transform.position;
        Vector2 beamEnd = beamStart + direction.normalized * beamRange;
        
        DrawBeam(beamStart, beamEnd);
        
        RaycastHit2D[] hits = Physics2D.BoxCastAll(beamStart, new Vector2(beamWidth, beamRange), 0, direction, beamRange, damageableLayer);

        foreach (var hit in hits)
        {
            hit.collider.gameObject.GetComponent<Asteroid>().TriggerDeath();
        }
        
        StartCoroutine(CleanupBeam());
    }

    private void DrawBeam(Vector2 start, Vector2 end)
    {
        if (beamLineRenderer != null)
        {
            beamLineRenderer.positionCount = 2;
            beamLineRenderer.SetPosition(0, start);
            beamLineRenderer.SetPosition(1, end);
        }
    }

    private IEnumerator CleanupBeam()
    {
        yield return new WaitForSeconds(0.1f); // Adjust beam duration as needed
        if (beamLineRenderer != null)
        {
            beamLineRenderer.positionCount = 0; // Clear the beam
        }
        Destroy(this.gameObject); // Destroy the projectile object
    }
    
    private void Explode()
    {
        Collider2D[] objectsInRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageableLayer);

        foreach (Collider2D obj in objectsInRadius)
        {
            obj.GetComponent<Asteroid>().TriggerDeath();
        }
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
    Beam,
    Boomerang,
    Missle,
    Pulse
}