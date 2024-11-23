using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public void Blink()
    {

        Vector3 cursorScreenPosition = Input.mousePosition;
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorScreenPosition);
        
        cursorWorldPosition.z = 0f;
        
        Player player = GameManager.Instance.player;
        player.playerRigidbody2D.position = cursorWorldPosition;
        
    }
    
    public void Burst()
    {
        Player player = GameManager.Instance.player;

        // Get the mouse position in world space
        Vector3 cursorScreenPosition = Input.mousePosition;
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorScreenPosition);

        cursorWorldPosition.z = 0f;

        // Calculate the direction from the player to the mouse position
        Vector2 direction = ((Vector2)cursorWorldPosition - player.playerRigidbody2D.position).normalized;

        // Apply the burst force in the calculated direction
        player.playerRigidbody2D.AddForce(direction * 5f, ForceMode2D.Impulse);
    }
}
