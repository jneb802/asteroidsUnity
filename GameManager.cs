using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public int lives = 3;
    public float respawnTime = 3f;
    public float respawnInvulnerabilityTime = 3f;
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
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

    private void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollision");
        this.player.gameObject.SetActive(true);
        
        Invoke(nameof(TurnOnCollision), respawnInvulnerabilityTime);
    }

    private void TurnOnCollision()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void GameOver()
    {
        // TODO
    }
    
}
