using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float trajectoryVariance = 15.0f;
    private float spawnRate = 2.0f;
    private float spawnDistance = 10.0f;
    public int spawnAmount = 2;
    
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitSphere.normalized * spawnDistance;
            Vector3 spawnPoint = this.transform.position + spawnDirection;
            
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion spawnRotation = Quaternion.AngleAxis(variance, Vector3.forward);
            
            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, spawnRotation);
            asteroid.size  =  Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(spawnRotation *  -spawnDirection);
        }
    }
}
