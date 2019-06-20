using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DinoStatsManager : MonoBehaviour
{
    int gold =0;
    int dinoStage =0;
    public const int MaxDinoStage = 3;
    int[] goldMultiplicativePerStage;
    int speedLevel = 0;
    public const int MaxSpeedLevel = 6;
    int smartnessLevel = 0;
    public const int MaxSmartnessLevel = 3;
    int generationLifespanLevel = 0;//  15 seg
    public const int MaxGenerationLifespanLevel = 7;// 120 seg
    int dinosPerGenerationLevel = 0; // 10
    public const int MaxDinosPerGenerationLevel = 10;//100
    int healthLevel = 0;
    public const int MaxHealthLevel = 10;    

    public int Gold { get => gold; set => gold = value; }
    public int DinoStage { get => dinoStage; set => dinoStage = (value <= MaxDinoStage?value:MaxDinoStage); }
    public int SpeedLevel { get => speedLevel; set => speedLevel = (value <= MaxSpeedLevel ? value : MaxSpeedLevel); }
    public int SmartnessLevel { get => smartnessLevel; set => smartnessLevel = (value <= MaxSmartnessLevel ? value : MaxSmartnessLevel); }
    public int GenerationLifespanLevel { get => generationLifespanLevel; set => generationLifespanLevel = (value <= MaxGenerationLifespanLevel ? value : MaxGenerationLifespanLevel); }
    public int DinosPerGenerationLevel { get => dinosPerGenerationLevel; set => dinosPerGenerationLevel = (value <= MaxDinosPerGenerationLevel ? value : MaxDinosPerGenerationLevel); }
    public int HealthLevel { get => healthLevel; set => healthLevel = (value <= MaxHealthLevel ? value : MaxHealthLevel); }
    public int GoldMultiplicative { get=> (goldMultiplicativePerStage[dinoStage]); }

    private void Awake()
    {
        Load();
        SaveLoad.BeforeClosing += Save;
    }
    private void Start()
    {
        goldMultiplicativePerStage = new int[] { 1, 2, 3, 15 }; // gold multiplicative scale
    }
    public void Save()
    {
        string path = SaveLoad.Instance.SaveDirectory + "Data.json";
        int[] data = new int[7];
        data[0] = gold;
        data[1] = dinoStage;
        data[2] = speedLevel;
        data[3] = smartnessLevel;
        data[4] = generationLifespanLevel;
        data[5] = dinosPerGenerationLevel;
        data[6] = healthLevel;
        DinoData saveData = new DinoData(data);
        string save = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, save);
    }
    void Load()
    {     
        if (SaveLoad.Instance.CheckSaveData("Data.json"))
        {
            string path = SaveLoad.Instance.SaveDirectory + "Data.json";
            DinoData saveData = JsonUtility.FromJson<DinoData>(File.ReadAllText(path));
            int[] data = saveData.data;
            gold = data[0];
            dinoStage = data[1];
            speedLevel = data[2];
            smartnessLevel = data[3];
            generationLifespanLevel = data[4];
            dinosPerGenerationLevel = data[5];
            healthLevel = data[6];
        }        
    }
    [System.Serializable]
    public class DinoData
    {
        public int[] data;
        public DinoData(int[] newData)
        {
            data = new int[newData.Length];
            for (int i = 0; i < newData.Length; i++)
            {
                data[i] = newData[i];
            }
        }  
    }  
}