using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoDirector
{
    GameObject nextObstacle = null;
    public GameObject NextObstacle { get { return nextObstacle; } set { nextObstacle = value; } }
    bool[] dinosAlive;
    public bool[] DinosAlive { get { return dinosAlive; }}
    static private InfoDirector instance = null;    
    static public InfoDirector Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InfoDirector();
            }
            return instance;
        }
    }

    public void KillDino(int dinoID)
    {
        dinosAlive[dinoID] = false;
    }
    public void resetDinos(int dinoPopulation)
    {
        dinosAlive = new bool[dinoPopulation];
        for (int i = 0; i < dinosAlive.Length; i++)
        {
            dinosAlive[i] = true;
        }
    }
    public float NextObstacleDistance()
    {
        if (nextObstacle != null)
        {
            return nextObstacle.transform.position.x;
        }
        else
        {
            return 5;
        }
    }
    public int NextObstacleType()
    {
        if (nextObstacle != null)
        {
            return nextObstacle.GetComponent<ObstacleBehaviour>().Type;
        }
        else
        {
            return -1;
        }
        
    }
}
