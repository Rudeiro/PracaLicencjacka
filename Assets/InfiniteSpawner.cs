using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> obstaclePrefabs;
    [SerializeField]
    List<float> initialObstaclesSpawnRate;
    [SerializeField]
    Vector2 spawnRange;
    [SerializeField]
    float initialFireRate;
    [SerializeField]
    int initialMagCapacity;
    [SerializeField]
    int initialEnemyHealth;
    [SerializeField]
    float initialEnemySpeed;
    private bool stopSpawning = false;
    private List<float> timesPassed = new List<float>();
    private float obstacleSpeed = 5f;
    private List<float> obstaclesSpawnRate = new List<float>();
    private float fireRate;
    private int magCapacity;
    private int enemyHealth;
    private float enemySpeed;

    public bool StopSpawning { get { return stopSpawning; } }

    private void Start()
    {
        for(int i = 0; i < obstaclePrefabs.Count; i++)
        {
            timesPassed.Add(0);
        }
        for (int i = 0; i < obstaclePrefabs.Count; i++)
        {
            obstaclesSpawnRate.Add(initialObstaclesSpawnRate[i]);
        }
    }

    public void ResetSpawner()
    {
        foreach (var obstacle in GetComponentsInChildren<Obstacle>())
        {
            Destroy(obstacle.gameObject);
        }
        foreach (var enemy in GetComponentsInChildren<EnemyAgent>())
        {
            enemy.WeaponHeld.WeaponReset();
            Destroy(enemy.gameObject);
        }
        fireRate = initialFireRate;
        magCapacity = initialMagCapacity;
        stopSpawning = false;
        obstacleSpeed = 5;
        enemyHealth = initialEnemyHealth;
        enemySpeed = initialEnemySpeed;
        //obstacleSpawnRate = 1;
        for(int i = 0; i < initialObstaclesSpawnRate.Count; i++)
        {
            obstaclesSpawnRate[i] = initialObstaclesSpawnRate[i];            
        }
        for (int i = 0; i < obstaclesSpawnRate.Count; i++)
        {
            timesPassed[i] = 0;
        }

    }
    
    void FixedUpdate()
    {
        if (!stopSpawning)
        {
            for (int i = 0; i < obstaclesSpawnRate.Count; i++)
            {
                if (timesPassed[i] >= obstaclesSpawnRate[i])
                {
                    timesPassed[i] = 0;
                    SpawnObstacle(obstaclePrefabs[i]);
                }
                else
                {
                    timesPassed[i] += Time.fixedDeltaTime;
                }
            }
        }
        
    }

    private void SpawnObstacle(GameObject obs)
    {
        
        
        int xPos = UnityEngine.Random.Range((int)spawnRange.x, (int)spawnRange.y);
        GameObject obstacle = Instantiate(obs, new Vector3(transform.position.x + xPos, transform.position.y, transform.position.z), Quaternion.identity);
        obstacle.transform.parent = transform;
        if (obstacle.GetComponent<Obstacle>() != null)
        {
            obstacle.GetComponent<Obstacle>().speed = obstacleSpeed;
        }
        if (obstacle.GetComponent<EnemyAgent>() != null)
        {
            stopSpawning = true;
            obstacle.GetComponent<EnemyAgent>().WeaponHeld.ChangeWeaponParameters(fireRate, magCapacity, enemyHealth, enemySpeed);
            obstacle.transform.Rotate(0, 180, 0);
            obstacle.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 6.5f);
        }
    }

    public void ChangeSpawnRateAndSpeed(float multiplier)
    {
        for (int i = 0; i < obstaclesSpawnRate.Count; i++)
        {
            obstaclesSpawnRate[i] /= multiplier;
        }
        obstacleSpeed *= multiplier;
        foreach (var obstacle in GetComponentsInChildren<Obstacle>())
        {
            obstacle.speed = obstacleSpeed;
        }
    }

    public void ChangeEnemyParameters(float rateMultiplier, float capacityMultiplier, float speedMultiplier)
    {
        fireRate /= rateMultiplier;
        magCapacity = (int) Mathf.Ceil( magCapacity*capacityMultiplier);
        enemyHealth = (int)Mathf.Ceil(enemyHealth * 1.5f);
        enemySpeed *= speedMultiplier;

    }

    public void Respawn(GameObject resp)
    {
        int xPos = UnityEngine.Random.Range((int)spawnRange.x, (int)spawnRange.y);
        resp.transform.position = new Vector3(transform.position.x + xPos, transform.position.y, transform.position.z);
    }

    public void ResumeSpawning()
    {
        stopSpawning = false;
    }
}
