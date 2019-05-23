using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{

    [SerializeField] float spawnrate = 1.0f;
    [SerializeField] float obstacleSpeed = 0.1f;
    [SerializeField] float spawnRandomDelayMax = 2.0f;
    [SerializeField] float endOfMap = -2.0f;
    float spawnCooldown = 0;
    int obstacleVariety = 1;
    List<GameObject> incomingObstacles;

    static private ObstaclesGenerator instance = null;
    static public ObstaclesGenerator Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);        
    }

    private void Start()
    {
        incomingObstacles = new List<GameObject>();
        spawnCooldown = 0;
    }

    void Update()
    {
        if (spawnCooldown <= 0)
        {
            Spawn();
            spawnCooldown = spawnrate + Random.Range(0, spawnRandomDelayMax);
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
        int typeToSpawn = Random.Range(0, obstacleVariety - 1);
        GameObject newObstacle = GetComponent<PoolManager>().RequestToPool(typeToSpawn , transform.position, transform.rotation);
        newObstacle.GetComponent<ObstacleBehaviour>().Speed = obstacleSpeed;
        newObstacle.GetComponent<ObstacleBehaviour>().EndOfMap = endOfMap;
        newObstacle.GetComponent<ObstacleBehaviour>().Pos = transform.position;
        newObstacle.GetComponent<ObstacleBehaviour>().Type = typeToSpawn;
        incomingObstacles.Add(newObstacle);
        if (incomingObstacles.Count == 0)
            InfoDirector.Instance.NextObstacleType = typeToSpawn;
    }
   
}
