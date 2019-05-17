using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    float speed;
    public float Speed { set { speed = value; } }   
    float endOfMap;
    public float EndOfMap { set { endOfMap = value; } }
    Vector3 pos = new Vector3(0, 0, 0);
    public Vector3 Pos { set { pos = value; } }
    int type;
    public int Type { get; set; }

    private void Update()
    {
        pos.x -= Time.deltaTime * speed;
        gameObject.transform.position = pos;
        if (gameObject.transform.position.x <= endOfMap)
            gameObject.SetActive(false);
    }
}
