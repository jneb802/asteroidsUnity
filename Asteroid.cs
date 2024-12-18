using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : Character
{
    public Sprite[] sprites;
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 3f;
    public float speed = 25.0f;
    public float maxLifetime = 30.0f;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    public GameObject landingZonePrefab;
    
    public float detectionRadius = 2f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        landingZonePrefab.SetActive(false);
    }

    private void Update()
    {
        if (size > 2)
        {
            Transform detectedTarget = DetectPlayer();
            
            if (detectedTarget != null)
            {
                landingZonePrefab.SetActive(true);
            }
        }
    }
    
    private Transform DetectPlayer()
    {
        Collider2D detectedCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Player"));

        if (detectedCollider != null)
        {
            return detectedCollider.transform;
        }

        return null;
    }
    
    private void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0,sprites.Length)];
        this.transform.eulerAngles = new Vector3(0, 0, Random.value * 360f);
        this.transform.localScale = Vector3.one * size;
        
        _rigidbody.mass = size * 2;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigidbody.AddForce(direction * speed);
    }

    
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            if (size * 0.5f > minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            TriggerDeath();
        }
    }

    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;
        
        Asteroid half = Instantiate(this,position,this.transform.rotation);
        half.size = size * 0.5f;
        half.SetTrajectory(Random.insideUnitCircle.normalized * speed);
    }
}
