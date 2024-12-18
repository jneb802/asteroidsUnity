using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public float respawnTime = 3f;
    public float respawnInvulnerabilityTime = 3f;
    public static GameManager Instance;
    public int lives = 3;
    public bool isPaused;

    private void Awake()
    {
        Instance = this;
    }

    private void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollision");
        this.player.shield = 100;
        player.statBars.SetValueBarSize("health",1);
        this.player.gameObject.SetActive(true);
        
        Invoke(nameof(TurnOnCollision), respawnInvulnerabilityTime);
    }

    private void TurnOnCollision()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void GameOver()
    {
        this.lives = 3;
        
        Invoke(nameof(Respawn), respawnTime);
    }
    
    public void PlayerDied()
    {
        if (lives < 0)
        {
            GameOver();
        }
        else
        {
            this.lives--;
            Invoke(nameof(Respawn),respawnTime);
        }
    }

    public void TogglePause(bool pause)
    {
        isPaused = pause;
        Time.timeScale = isPaused ? 0 : 1; // 0 for paused, 1 for unpaused
    }
    
}
