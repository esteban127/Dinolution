using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoBehaviour : MonoBehaviour
{
    [SerializeField] float baseActionDuration = 1;    
    [SerializeField] float jumpHeight = 1;
    [SerializeField] float obstacleJumpHeight = 0.2f;
    [SerializeField] float obstacleWidth = 0.2f;
    [SerializeField] float bestActionDistance = 0.5f;

    float actionDuration;
    bool rendering = false;
    public bool Rendering { set { rendering = value; } }
    int infoLentght = 0;
    int actionLentght = 0;
    float actionTime = 0;
    bool alive = true;
    bool crouching = false;
    Vector3 pos = new Vector3 (0,0,0);
    NeuronalNetwork myNeuronalNetwork;
    float fitness = 0;
    delegate void ActionDelegate();
    ActionDelegate act;
    float[] information;
    float[] actions;
    InfoDirector infoInstance;

    private void Start()
    {
        act += Think;
        information = new float[infoLentght];
        actions = new float[actionLentght];
        infoInstance = InfoDirector.Instance;
    }    

    void Update()
    {
        UpdateInfo();
        CheckObstacles();
        if (alive)
        {
            fitness += Time.deltaTime;
            act();
        }
    }

    public void SetSpeed(float speed)
    {
        actionDuration = baseActionDuration * (1/speed);
    }

    private void CheckObstacles()
    {
        if (infoInstance.NextObstacleDistance <= obstacleWidth)
        {
            switch (infoInstance.NextObstacleType)
            {
                case 0:
                    if (transform.position.y < obstacleJumpHeight)
                    {
                        Die();
                    }                                           
                    break;
                case 1:                    
                    if (!crouching)
                    {
                        Die();
                    }                                        
                    break;
            }
        }
    }
    private void UpdateInfo()
    {
        for (int i = 0; i < information.Length; i++)
        {
            switch (i)
            {
                case 0:
                    information[i] = infoInstance.NextObstacleDistance;
                    break;
                case 1:
                    information[i] = infoInstance.NextObstacleType*2;
                    break;
            }
        }
    }

    private void Think()
    {
        actions = myNeuronalNetwork.FitFoward(information);
        if (actions[0] < actions[1])
        {
            act += Jump;
            act -= Think;
            GiveFitness(0);
        }
        else
        {
            if (actions.Length > 2 && actions[0] < actions[2])
            {
                if (rendering)
                    GetComponentInChildren<AnimationBehaviour>().Courch();
                crouching = true;
                act += Crouching;
                act -= Think;
                GiveFitness(1);
            }
        }
    }

    void Jump()
    {
        actionTime += Time.deltaTime;
        if(actionTime < actionDuration)
            pos.y = (actionDuration * actionTime - Mathf.Pow(actionTime, 2))*jumpHeight/ Mathf.Pow(actionDuration, 2);
        else
        {
            actionTime = 0;
            pos.y = 0;
            act -= Jump;
            act += Think;
        }
        transform.position = pos;
    }    

    private void GiveFitness(int actionID)
    {
        if(actionID == infoInstance.NextObstacleType)
        {            
            fitness += (20 - Math.Abs(bestActionDistance - infoInstance.NextObstacleDistance) * 10);                
        } 
    }

    public void Reset(NeuronalNetwork neuNet, int[] NeuSize)
    {
        myNeuronalNetwork = neuNet;
        infoLentght = NeuSize[0];
        alive = true;        
        actionTime = 0;
        fitness = 0;
        crouching = false;
        gameObject.SetActive(true);
    }
    
    void Crouching()
    {
        actionTime += Time.deltaTime;
        if (actionTime >= actionDuration)
        {
            actionTime = 0;
            crouching = false;
            if (rendering)
                GetComponentInChildren<AnimationBehaviour>().ReturnToIdle();
            act -= Crouching;
            act += Think;
        }
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
