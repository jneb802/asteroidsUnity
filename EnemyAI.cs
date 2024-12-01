using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 200f;
    public float detectionRadius = 10f;
    private Transform target;
    private Rigidbody2D _rigidbody;
    private bool isAttacking = false;
    private Character _character;
    public float attackCooldown = 1.5f;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
    }

    private void FixedUpdate()
    {
        DetectPlayer();
        
        if (target == null)
        {
            return;
        }
        
        Vector2 direction = (target.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.fixedDeltaTime / 360f);
        
        _rigidbody.rotation = currentAngle;
        // _rigidbody.velocity = transform.right * speed;
        
        if (!isAttacking)
        {
            StartCoroutine(AttackPlayer());
        }
    }
    
    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        while (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) <= detectionRadius)
            {
                this._character.FireProjectile(_character.primaryWeapon, target.position);
            }
            
            yield return new WaitForSeconds(attackCooldown);
        }
        isAttacking = false;
    }
    
    private void DetectPlayer()
    {
        Collider2D detectedCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Player"));

        if (detectedCollider != null)
        {
            target = detectedCollider.transform;
        }
        else
        {
            target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the Scene view for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}