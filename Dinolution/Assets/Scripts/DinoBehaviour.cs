using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoBehaviour : MonoBehaviour
{
    [SerializeField] float jumpDuration = 2;
    [SerializeField] float jumpHeight = 1;
    [SerializeField] float obstacleJumpHeight = 0.2f;
    [SerializeField] float obstacleWidth = 0.2f;
    float currentJumpTime = 0;
    bool alive = true;
    Vector3 pos = new Vector3 (0,0,0);
    NeuronalNetwork myNeuronalNetwork;
    float fitness = 0;
    delegate void ActionDelegate();
    ActionDelegate act;
    float[] information;
    float[] actions;

    private void Start()
    {
        act += Think;
        information = new float[1];
        actions = new float[2];
    }    

    void Update()
    {
        information[0] = InfoDirector.Instance.NextObstacleDistance;
        if (information[0] <= obstacleWidth)
        {
            switch (InfoDirector.Instance.NextObstacleType)
            {
                case 0:
                    if (transform.position.y < obstacleJumpHeight)                    
                        Die();
                    else
                        fitness += 25;                    
                    break;
            }
        }
        if (alive)
        {
            act();
        }
             
       
    }

    void Jump()
    {
        currentJumpTime += Time.deltaTime;
        if(currentJumpTime<jumpDuration)
            pos.y = (jumpDuration * currentJumpTime - Mathf.Pow(currentJumpTime, 2))*jumpHeight/ Mathf.Pow(jumpDuration, 2);
        else
        {
            currentJumpTime = 0;
            pos.y = 0;
            act -= Jump;
            act += Think;
        }
        fitness--;
        transform.position = pos;
    }

    private void Think()
    {
        actions = myNeuronalNetwork.FitFoward(information);
        if (actions[0] < actions[1])
        {
            act += Jump;
            act -= Think;
        }        
    }

    public void ReciveNeuronalNetwork(NeuronalNetwork neuNet)
    {
        myNeuronalNetwork = neuNet;
        alive = true;
        gameObject.SetActive(true);
    }
    
    void Die()
    {
        alive = false;
        gameObject.SetActive(false);
    }
    public void CalculateFitness()
    {
        myNeuronalNetwork.SetFitness(fitness);
    }

}
