using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopEvents : MonoBehaviour
{
    [SerializeField] DinoDisplayInShop dinoDisplay = null;
    [SerializeField] DinoStatsManager stats=null;    
    [SerializeField] ShopButtonManager stageButton=null;
    [SerializeField] ShopButtonManager speedButton = null;
    [SerializeField] ShopButtonManager smartnessButton = null;
    [SerializeField] ShopButtonManager lifespanButton = null;
    [SerializeField] ShopButtonManager populationButton = null;
    [SerializeField] ShopButtonManager healthButton = null;
    [SerializeField] Text goldText = null;
    SaveLoad SLManager;

    private void Start()
    {
        SLManager = SaveLoad.Instance;
        ActualizateUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SLManager.ChangeScene("MainMenu");
        }
    }
    public void StartGame()
    {
        SLManager.ChangeScene("DinoLand");
    }

    private void ActualizateUI()
    {
        dinoDisplay.selectDino(stats.DinoStage);

        stageButton.ActualizateButton(stats.DinoStage, stats.Gold);
        speedButton.ActualizateButton(stats.SpeedLevel, stats.Gold);
        smartnessButton.ActualizateButton(stats.SmartnessLevel, stats.Gold);
        lifespanButton.ActualizateButton(stats.GenerationLifespanLevel, stats.Gold);
        populationButton.ActualizateButton(stats.DinosPerGenerationLevel, stats.Gold);
        healthButton.ActualizateButton(stats.HealthLevel, stats.Gold);

        goldText.text = stats.Gold.ToString();
    }

    public void Purchase(int statID)
    {
        switch (statID)
        {
            case 0:                
                stats.DinoStage++;
                stats.Gold -= stageButton.ActualPrice;
                break;
            case 1:
                stats.SpeedLevel++; 
                stats.Gold -= speedButton.ActualPrice;
                break;
            case 2:
                stats.SmartnessLevel++;
                stats.Gold -= smartnessButton.ActualPrice;
                break;
            case 3:
                stats.GenerationLifespanLevel++;
                stats.Gold -= lifespanButton.ActualPrice;
                break;
            case 4:
                stats.DinosPerGenerationLevel++;
                stats.Gold -= populationButton.ActualPrice;
                break;
            case 5:
                stats.HealthLevel++;
                stats.Gold -= healthButton.ActualPrice;
                break;
        }
        ActualizateUI();
    }
}
