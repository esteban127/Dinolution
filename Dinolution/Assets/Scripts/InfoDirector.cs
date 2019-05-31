using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoDirector
{
    float nextObstacleDistance;
    public float NextObstacleDistance { get; set; }
    int nextObstacleType;
    public int NextObstacleType { get; set; }
    float generationLifetime;
    public float GenerationLifetime { get; set; }

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
}
