using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DinoStatsManager : MonoBehaviour
{
    int gold =0;
    int dinoStage =0;
    public const int MaxDinoStage = 4;
    float[] goldMultiplicativePerStage;
    int speedLevel = 0;
    public const int MaxSpeedLevel = 4;
    int smartness = 0;
    public const int MaxSmartness = 3;
    float generationLifespan = 15.0f;
    public const float MaxGenerationLifespan = 120.0f;
    int dinosPerGeneration = 10;
    public const int MaxDinosPerGeneration = 150;
    float health = 1.0f;
    public const float MaxHealth = 20.0f;    

    public int Gold { get => gold; set => gold = value; }
    public int DinoStage { get => dinoStage; set => dinoStage = value; }
    public int SpeedLevel { get => speedLevel; set => speedLevel = value; }
    public int Smartness { get => smartness; set => smartness = value; }
    public float GenerationLifespan { get => generationLifespan; set => generationLifespan = value; }
    public int DinosPerGeneration { get => dinosPerGeneration; set => dinosPerGeneration = value; }
    public float Health { get => health; set => health = value; }
    public float GoldMultiplicative { get=> (goldMultiplicativePerStage[dinoStage]); }

    private void Awake()
    {
        Load();
        SaveLoad.BeforeClosing += Save;
    }
    private void Start()
    {
        goldMultiplicativePerStage = new float[] { 1, 2, 3, 15 }; // gold multiplicative scale
    }
    public void Save()
    {
        string path = SaveLoad.Instance.SaveDirectory + "Data.json";
        float[] data = new float[7];
        data[0] = gold;
        data[1] = dinoStage;
        data[2] = speedLevel;
        data[3] = smartness;
        data[4] = generationLifespan;
        data[5] = dinosPerGeneration;
        data[6] = health;
        DinoData saveData = new DinoData(data);
        string save = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, save);
    }
    void Load()
    {     
        if (SaveLoad.Instance.CheckSaveData())
        {
            string path = SaveLoad.Instance.SaveDirectory + "Data.json";
            DinoData saveData = JsonUtility.FromJson<DinoData>(File.ReadAllText(path));
            float[] data = saveData.data;
            gold = (int)data[0];
            dinoStage = (int)data[1];
            speedLevel = (int)data[2];
            smartness = (int)data[3];
            generationLifespan = data[4];
            dinosPerGeneration = (int)data[5];
            health = data[6];
        }        
    }
    [System.Serializable]
    public class DinoData
    {
        public float[] data;
        public DinoData(float[] newData)
        {
            data = new float[newData.Length];
            for (int i = 0; i < newData.Length; i++)
            {
                data[i] = newData[i];
            }
        }  
    }  
}