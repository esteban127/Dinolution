using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DinoLandDirector : MonoBehaviour
{
    [SerializeField] float deathDelay = 1.5f;
    [SerializeField] ConfirmPanel confirm = null;
    [SerializeField] DinoGenerator dinoG = null;
    [SerializeField] ObstaclesGenerator obstaclesG = null;
    float speed = 1;
    [SerializeField] DinoStatsManager stats = null;
    [SerializeField] Text goldText = null;
    [SerializeField] Text scoreText = null;
    [SerializeField] Text populationText = null;
    [SerializeField] Slider lifespan = null;
    float counter = 0;
    float diedCounter = 0;

    SaveLoad SLManager;
    private void Start()
    {
        SLManager = SaveLoad.Instance;               
        int[] neuNetwork = CreateNeuronalNetworkSize();
        dinoG.Initalzie(10 + (stats.DinosPerGenerationLevel*10), neuNetwork,stats.DinoStage);
        obstaclesG.ObstacleVariety = stats.DinoStage + 1;
        obstaclesG.DinoPopulation = (10 + (stats.DinosPerGenerationLevel * 10));
        goldText.text = stats.Gold.ToString();
        SetSpeed(1);
    }
    private void Awake()
    {
        SaveLoad.BeforeClosing += SaveGeneration;
    }

    private void SaveGeneration()
    {
        int[] neuNetwork = CreateNeuronalNetworkSize();
        dinoG.Save((10 + (stats.DinosPerGenerationLevel * 10)), neuNetwork);
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

        if (counter > (15 + stats.GenerationLifespanLevel*15)||diedCounter>deathDelay)
        {
            obstaclesG.Reset();
            dinoG.NewGeneration();
            stats.Gold += (int)((counter + (counter*counter)/100) * stats.GoldMultiplicative);
            goldText.text = stats.Gold.ToString();
            counter = 0;
            diedCounter = 0;
        }
        ActalizateUI();
        if (dinoG.CheckExtinction())
        {
            diedCounter+= (Time.deltaTime * speed);
        }
        else
        {
            counter += (Time.deltaTime * speed);
        }        
    }

    private void ActalizateUI()
    {
        scoreText.text = "Score: " + (int)((counter+ (counter * counter) / 100));
        populationText.text = "Population: " + InfoDirector.Instance.AmountOfDinosAlive();
        lifespan.value = (((15 + stats.GenerationLifespanLevel * 15)- counter) / (15 + stats.GenerationLifespanLevel * 15));
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
