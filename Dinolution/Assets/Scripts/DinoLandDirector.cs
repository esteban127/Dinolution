using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DinoLandDirector : MonoBehaviour
{
    [SerializeField] ConfirmPanel confirm = null;
    [SerializeField] DinoGenerator dinoG = null;
    [SerializeField] ObstaclesGenerator obstaclesG = null;
    float speed = 1;
    [SerializeField] DinoStatsManager stats = null;
    [SerializeField] Text goldText = null;
    float counter = 0;

    SaveLoad SLManager;
    private void Start()
    {
        SLManager = SaveLoad.Instance;               
        int[] neuNetwork = CreateNeuronalNetworkSize();
        dinoG.Initalzie(10 + (stats.DinosPerGenerationLevel*15), neuNetwork);
        obstaclesG.ObstacleVariety = stats.DinoStage + 1;
        goldText.text = stats.Gold.ToString();
        SetSpeed(1);
    }

    private int[] CreateNeuronalNetworkSize()
    {
        int[] neuNetwork = new int[6 - stats.SmartnessLevel];
        for (int i = 0; i < 4 - stats.SmartnessLevel; i++)
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

    public void ActualziateSpeed(float percentage)
    {
        SetSpeed(1 + percentage * stats.SpeedLevel * 0.5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(WaitForConfirm());
        }

        if (counter > (15 + stats.GenerationLifespanLevel*15)|| dinoG.CheckExtinction())
        {
            obstaclesG.Reset();
            dinoG.NewGeneration();
            stats.Gold += (int)(counter * stats.GoldMultiplicative);
            goldText.text = stats.Gold.ToString();
            counter = 0;
        }
        InfoDirector.Instance.GenerationLifetime = counter;
        counter += (Time.deltaTime * speed);
    }

    public void ReturnToShop()
    {
        SLManager.ChangeScene("Shop");
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
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
