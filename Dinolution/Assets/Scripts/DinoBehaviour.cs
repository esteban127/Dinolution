using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoBehaviour : MonoBehaviour
{
    [SerializeField] float jumpDuration = 2;
    [SerializeField] float jumpHeight = 1;
    float currentJumpTime = 0;
    Vector3 pos = new Vector3 (0,0,0);
    NeuronalNetwork myNeuronalNetwork;
    public float fitnes;
    delegate void ActionDelegate();
    ActionDelegate action;
    // Update is called once per frame

    private void Start()
    {
        action += Think;
    }    

    void Update()
    {
        action();   
        if(Input.GetKeyDown(KeyCode.Space))
            action += Jump;
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
            action -= Jump;
            action += Think;
        }  
        transform.position = pos;
    }

    private void Think()
    {
        if(InfoDirector.Instance.NextObstacleDistance<=0.5)
        {
            action += Jump;
            action -= Think;
        }

            
    }

    public void ReciveNeuronalNetwork(NeuronalNetwork neuNet)
    {
        myNeuronalNetwork = neuNet;
    }
    
    public void Die(float deathTime)
    {
        fitnes = deathTime;
    }


}
