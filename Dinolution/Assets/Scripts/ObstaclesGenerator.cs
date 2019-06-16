using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{

    [SerializeField] float spawnrate = 1.0f;    
    [SerializeField] float baseObstacleSpeed = 0.1f;
    float speed = 1;
    float obstacleSpeed;
    [SerializeField] float endOfMap = -2.0f;
    float spawnCooldown = 0;
    int dinoPopulation = 10;
    int obstacleVariety = 1;
    public int ObstacleVariety { get{ return obstacleVariety; } set { obstacleVariety = value; } }
    public int DinoPopulation { get { return dinoPopulation; } set { dinoPopulation = value; } }
    List<GameObject> incomingObstacles;    

    private void Start()
    {
        incomingObstacles = new List<GameObject>();
        spawnCooldown = 0;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        obstacleSpeed = baseObstacleSpeed *speed;
        foreach(GameObject obstacle in GetComponent<PoolManager>().getAllActiveObjects())
        {
            obstacle.GetComponent<ObstacleBehaviour>().Speed = obstacleSpeed;
        }
    }

    void Update()
    {
        if (spawnCooldown <= 0)
        {
            Spawn();
            spawnCooldown = spawnrate + Random.Range(0, spawnrate*2);
        }
        spawnCooldown -= Time.deltaTime*speed;
        if (incomingObstacles.Count > 0)
        {
            if (incomingObstacles[0].transform.position.x > 0)
            {
                if(InfoDirector.Instance.NextObstacle!= incomingObstacles[0])
                    InfoDirector.Instance.NextObstacle = incomingObstacles[0];
            }
            else
            {
                if (incomingObstacles.Count > 1)
                {
                    InfoDirector.Instance.NextObstacle = incomingObstacles[1];
                }
                InfoDirector.Instance.NextObstacle = null;                
                incomingObstacles.RemoveAt(0);
            }
        }
        else
        {
            InfoDirector.Instance.NextObstacle = null;
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
        if(typeToSpawn == 2) //destructible object
        {
            newObstacle.GetComponent<DestructibleObstacleBehaviour>().ResetStatus(dinoPopulation);
        }
        if (incomingObstacles.Count == 0)
        {
            InfoDirector.Instance.NextObstacle = newObstacle;
        }
        incomingObstacles.Add(newObstacle);

    }
   
}
