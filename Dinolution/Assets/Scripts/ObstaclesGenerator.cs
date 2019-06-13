using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{

    [SerializeField] float baseSpawnrate = 1.0f;
    float spawnrate;
    [SerializeField] float baseObstacleSpeed = 0.1f;
    float obstacleSpeed;
    [SerializeField] float endOfMap = -2.0f;
    float spawnCooldown = 0;
    int obstacleVariety = 1;
    public int ObstacleVariety { get{ return obstacleVariety; } set { obstacleVariety = value; } }
    List<GameObject> incomingObstacles;    

    private void Start()
    {
        incomingObstacles = new List<GameObject>();
        spawnCooldown = 0;
    }

    public void SetSpeed(float speed)
    {
        spawnrate = baseSpawnrate * (1/speed);
        obstacleSpeed = baseObstacleSpeed *speed;
    }

    void Update()
    {
        if (spawnCooldown <= 0)
        {
            Spawn();
            spawnCooldown = spawnrate + Random.Range(0, spawnrate*2);
        }
        spawnCooldown -= Time.deltaTime;
        if (incomingObstacles.Count > 0)
        {
            if (incomingObstacles[0].transform.position.x > 0)
            {
                InfoDirector.Instance.NextObstacleDistance = incomingObstacles[0].transform.position.x;
            }
            else
            {
                if (incomingObstacles.Count > 1)
                {
                    InfoDirector.Instance.NextObstacleType = incomingObstacles[1].GetComponent<ObstacleBehaviour>().Type;
                }
                InfoDirector.Instance.NextObstacleDistance = 2;                
                incomingObstacles.RemoveAt(0);
            }
        }
        else
        {
            InfoDirector.Instance.NextObstacleDistance = 2;
        }
            
    }

    public void Reset()
    {
        incomingObstacles.Clear();
        spawnCooldown = 0;
        GetComponent<PoolManager>().DeleteAll();
    }
    
    void Spawn()
    {
        int typeToSpawn = Random.Range(0, obstacleVariety);
        GameObject newObstacle = GetComponent<PoolManager>().RequestToPool(typeToSpawn , transform.position, transform.rotation);
        newObstacle.GetComponent<ObstacleBehaviour>().Speed = obstacleSpeed;
        newObstacle.GetComponent<ObstacleBehaviour>().EndOfMap = endOfMap;
        newObstacle.GetComponent<ObstacleBehaviour>().Pos = transform.position;
        newObstacle.GetComponent<ObstacleBehaviour>().Type = typeToSpawn; 
        if (incomingObstacles.Count == 0)
        {
            InfoDirector.Instance.NextObstacleDistance = transform.position.x;
            InfoDirector.Instance.NextObstacleType = typeToSpawn;
        }
        incomingObstacles.Add(newObstacle);

    }
   
}
