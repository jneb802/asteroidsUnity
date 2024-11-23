using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    public ParticleSystem deathEffect;
    public Sprite[] sprites;
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 25.0f;
    public float maxLifetime = 30.0f;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    
    public GameObject lootPrefab;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
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

    public void TriggerDeath()
    {
        ParticleSystem effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        effect.Play();
            
        Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constant);
            
        float randomNumber = Random.value;

        if (randomNumber < 0.95f)
        {
            if (lootPrefab != null)
            {
                GameObject cargoCrate = Instantiate(lootPrefab, transform.position, Quaternion.identity);
                LootDrop lootDrop = cargoCrate.GetComponent<LootDrop>();
                if (lootDrop != null)
                {
                    lootDrop.SetTrajectory(Random.insideUnitCircle.normalized);
                }
            } 
        }
        
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
