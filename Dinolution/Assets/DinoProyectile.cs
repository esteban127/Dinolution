using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoProyectile : MonoBehaviour
{
    int dinoID = 0;
    float speed = 1;
    float obstacleWidth;
    Vector3 pos = new Vector3(0, 0, 0);
    bool proyectileFlying = false;
    public bool ProyectileFlying { get { return proyectileFlying; } }
    [SerializeField] float baseProyectileSpeed = 1.0f;
    [SerializeField] float endOfMap = 2.0f;

    private void Update()
    {
        pos.x += Time.deltaTime * baseProyectileSpeed * speed;
        gameObject.transform.position = pos;
        if (gameObject.transform.position.x >= endOfMap)
        {
            proyectileFlying = false;
            gameObject.SetActive(false);
        }
        if(InfoDirector.Instance.NextObstacleDistance()<= (pos.x+obstacleWidth))
        {
            if (InfoDirector.Instance.NextObstacleType() == 2)
            {
                InfoDirector.Instance.NextObstacle.GetComponent<DestructibleObstacleBehaviour>().Destroy(dinoID);
            }
            proyectileFlying = false;
            gameObject.SetActive(false);
        }
    }

    public void Instance(int newDinoID, float newSpeed, Vector3 newPos, float newObstacleWidth)
    {
        gameObject.SetActive(true);
        proyectileFlying = true;
        dinoID = newDinoID;
        speed = newSpeed;
        pos = newPos;
        obstacleWidth = newObstacleWidth;
    }

}
