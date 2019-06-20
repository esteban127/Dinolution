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
    [SerializeField] float outOfBounds = -2.5f;

    float actionDuration;
    int dinoID = 0;
    float speed = 1;
    public int DinoID { set { dinoID = value; } }
    bool rendering = false;
    public bool Rendering { set { rendering = value; } }
    int infoLentght = 0;
    int actionLentght = 0;
    int actionToMake = 0;    
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

    private void Awake()
    {
        infoInstance = InfoDirector.Instance;
    }
    private void Start()
    {
        act += Think;
        information = new float[infoLentght];
        actions = new float[actionLentght];
    }    

    void Update()
    {
        UpdateInfo();
        CheckObstacles();
        if (alive)
        {
            fitness += Time.deltaTime;
        }
        act?.Invoke();
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        actionDuration = baseActionDuration * (1/speed);
    }

    private void CheckObstacles()
    {
        if (infoInstance.NextObstacleDistance() <= obstacleWidth)
        {
            switch (infoInstance.NextObstacleType())
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
                case 2:
                    if (!infoInstance.NextObstacle.GetComponent<DestructibleObstacleBehaviour>().CheckDestroyed(dinoID))
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
                    information[i] = infoInstance.NextObstacleDistance();
                    break;
                case 1:
                    information[i] = infoInstance.NextObstacleType();
                    break;
            }
        }
    }

    private void Think()
    {
        actions = myNeuronalNetwork.FitFoward(information);
        actionToMake = 0;
        for (int i = 1; i < actions.Length; i++)
        {
            if(actions[i]> actions[actionToMake])
            {
                actionToMake = i;
            }
        }

        switch (actionToMake)
        {
            case 1: //jump
                act += Jump;
                act -= Think;
                GiveFitness(0);
                break;
            case 2: //courch
                if (rendering)
                    GetComponentInChildren<AnimationBehaviour>().Courch();
                crouching = true;
                act += Crouching;
                act -= Think;
                GiveFitness(1);
                break;
            case 3: //shoot
                if (!GetComponentInChildren<DinoProyectile>(true).ProyectileFlying)
                {
                    GetComponentInChildren<DinoProyectile>(true).Instance(dinoID, speed, transform.position, obstacleWidth);
                    if (rendering)
                        GetComponentInChildren<AnimationBehaviour>().Shoot();
                    act += Shooting;
                    act -= Think;
                    GiveFitness(2);
                }
                break;

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
    void Shooting()
    {
        actionTime += Time.deltaTime;
        if (actionTime >= actionDuration/2)
        {
            actionTime = 0;
            if (rendering)
                GetComponentInChildren<AnimationBehaviour>().ReturnToIdle();
            act -= Shooting;
            act += Think;
        }
    }

    void Die()
    {
        infoInstance.KillDino(dinoID);
        alive = false;
        CleanDelegate();
        act += Dying;
        pos.y = 0;
        if(rendering)
            GetComponentInChildren<AnimationBehaviour>().Die();
        transform.position = pos;
    }
    void Dying()
    {
        pos.x -= Time.deltaTime * speed;
        if (pos.x<=outOfBounds)
        {
            if (rendering)
                GetComponentInChildren<AnimationBehaviour>().ReturnToIdle();
            act -= Dying;
            act += Think;
            gameObject.SetActive(false);
        }
        transform.position = pos;
    }
    private void CleanDelegate()
    {
        Delegate[] functions = act.GetInvocationList();
        for (int i = 0; i < functions.Length; i++)
        {
            act -= (ActionDelegate)functions[i];
        }
    }

    private void GiveFitness(int actionID)
    {        
        if (actionID == infoInstance.NextObstacleType())
        {
            fitness += (20 - Math.Abs(bestActionDistance - infoInstance.NextObstacleDistance()) * 10);
        }
    }

    public void Reset(NeuronalNetwork neuNet, int infoLeng)
    {
        myNeuronalNetwork = neuNet;
        infoLentght = infoLeng;
        alive = true;        
        actionTime = 0;
        infoInstance.ReviveDino(dinoID);
        fitness = 0;
        crouching = false;
        pos = new Vector3(0, 0, 0);
        transform.position = pos;
        if (act!= null)
        {
            CleanDelegate();
            act += Think;
        }
        if (rendering)
            GetComponentInChildren<AnimationBehaviour>().ReturnToIdle();
        gameObject.SetActive(true);
    }  

    public void CalculateFitness()
    {
        myNeuronalNetwork.SetFitness(fitness);
    }
    public NeuronalNetwork.SavedNeuronalNetwork SaveNeuronalNetwork()
    {
        return myNeuronalNetwork.SaveNeuronalNetwork();
    }
    public void LoadNeuronalNetwork(int[] initilLayers, NeuronalNetwork.SavedNeuronalNetwork save)
    {
        myNeuronalNetwork.LoadNeuronalnetwork(initilLayers, save);
    }
}
