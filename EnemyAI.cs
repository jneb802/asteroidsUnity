using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 200f;
    public Transform target;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("EnemyAI: No target assigned!");
            return;
        }

        // Calculate direction toward the target
        Vector2 direction = (target.position - transform.position).normalized;

        // Rotate smoothly toward the target
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.fixedDeltaTime / 360f);

        // Apply the rotation to the Rigidbody2D
        _rigidbody.rotation = currentAngle;

        // Move forward in the direction the enemy is facing
        _rigidbody.velocity = transform.right * speed;
    }
}