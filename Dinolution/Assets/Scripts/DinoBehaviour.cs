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
    [SerializeField] float crouchDuration = 0.2f;
    [SerializeField] float bestActionDistance = 0.5f;
    bool alphaBoy = false;
    public bool AlphaBoy { set { alphaBoy = value; } }
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
                if (alphaBoy)
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
        if(actionTime < jumpDuration)
            pos.y = (jumpDuration * actionTime - Mathf.Pow(actionTime, 2))*jumpHeight/ Mathf.Pow(jumpDuration, 2);
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
            if(Math.Abs(bestActionDistance - infoInstance.NextObstacleDistance) < 0.15f)
            {
                fitness += (20 - Math.Abs(bestActionDistance - infoInstance.NextObstacleDistance) * 10);
            }        
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
        if (actionTime >= crouchDuration)
        {
            actionTime = 0;
            crouching = false;
            if (alphaBoy)
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
        if (alphaBoy)
            Debug.Log(fitness);
    }

}
