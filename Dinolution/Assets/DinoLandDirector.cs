using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoLandDirector : MonoBehaviour
{
    [SerializeField] ConfirmPanel confirm = null;
    [SerializeField] DinoGenerator dinoG = null;
    [SerializeField] ObstaclesGenerator obstaclesG = null;
    [SerializeField, Range(1, 5)] float speed = 1;
    [SerializeField] DinoStatsManager stats = null;
    float counter = 0;

    SaveLoad SLManager;
    private void Start()
    {
        SLManager = SaveLoad.Instance;               
        int[] neuNetwork = CreateNeuronalNetworkSize();
        dinoG.Initalzie(stats.DinosPerGeneration, neuNetwork);
        obstaclesG.ObstacleVariety = stats.DinoStage + 1;
        SetSpeed();
    }

    private int[] CreateNeuronalNetworkSize()
    {
        int[] neuNetwork = new int[6 - stats.Smartness];
        for (int i = 0; i < 4 - stats.Smartness; i++)
        {
            neuNetwork[i + 1] = 5;
        }
        if (stats.DinoStage<4)
        {
            neuNetwork[0] = stats.DinoStage + 1;
        }// agregar en fase 4 para final
        neuNetwork[neuNetwork.Length - 1] = stats.DinoStage + 2;
        return neuNetwork;
            
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(WaitForConfirm());
        }

        if (counter > stats.GenerationLifespan || dinoG.CheckExtinction())
        {
            obstaclesG.Reset();
            dinoG.NewGeneration();
            counter = 0;
        }
        InfoDirector.Instance.GenerationLifetime = counter;
        counter += (Time.deltaTime * speed);
    }

    public void ReturnToShop()
    {
        SLManager.ChangeScene("Shop");
    }

    public void SetSpeed()
    {
        dinoG.SetSpeed(speed);
        obstaclesG.SetSpeed(speed);
    }

    IEnumerator WaitForConfirm()
    {
        confirm.Ask();
        while (confirm.ConfirmResult == "Null")
        {
            yield return null;
        }
        if (confirm.ConfirmResult == "Yes")
        {
            SLManager.ChangeScene("MainMenu");
        }
    }
}
