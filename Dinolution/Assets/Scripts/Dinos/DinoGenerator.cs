using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class DinoGenerator : MonoBehaviour
{
    List<NeuronalNetwork> NeuronalList;
    public int generation = 0;
    [SerializeField] GameObject dinoPrefab = null;
    [SerializeField] GameObject dinoStyle = null;
    [SerializeField] GameObject dinoProyectile = null;
    GameObject[] poblation;
    int infoLenght = 1;
    
    public void Initalzie(int poblationNum, int[] neuronalNetworkSize, int dinoStage)
    {
        infoLenght = neuronalNetworkSize[0];
        poblation = new GameObject[poblationNum];
        NeuronalList = new List<NeuronalNetwork>();
        InfoDirector.Instance.SetDinos(poblationNum);
        for (int i = 0; i < poblationNum; i++)
        {
            GameObject myDino = Instantiate(dinoPrefab);
            NeuronalNetwork myIA = new NeuronalNetwork(neuronalNetworkSize);
            NeuronalList.Add(myIA);
            myDino.GetComponent<DinoBehaviour>().Reset(myIA, infoLenght);
            myDino.transform.position = transform.position;
            GameObject style = Instantiate(dinoStyle);
            myDino.GetComponent<DinoBehaviour>().Rendering = true; //render all
            myDino.GetComponent<DinoBehaviour>().DinoID = i;
            style.GetComponent<AnimationBehaviour>().DinoStage = dinoStage;
            style.transform.SetParent(myDino.transform);
            if (dinoStage == 2)
            {
                GameObject proyectile = Instantiate(dinoProyectile);
                proyectile.transform.SetParent(myDino.transform);
            }
            poblation[i] = myDino;
            myDino.transform.SetParent(transform);
        }
        TryToLoad(poblationNum,neuronalNetworkSize);
    }

    public void Save(int poblationNum, int[] neuronalNetworkSize)
    {
        string path = SaveLoad.Instance.SaveDirectory + "NeuronalNetwork.json";
        NeuronalNetwork.SavedNeuronalNetwork[] savedNeuronalNetworks = new NeuronalNetwork.SavedNeuronalNetwork[poblationNum];        
        for (int i = 0; i < poblation.Length; i++)
        {
            savedNeuronalNetworks[i] = poblation[i].GetComponent<DinoBehaviour>().SaveNeuronalNetwork();
        }
        SavedGeneration savedGen = new SavedGeneration(savedNeuronalNetworks, neuronalNetworkSize, poblationNum);
        string save = JsonUtility.ToJson(savedGen);
        File.WriteAllText(path, save);
    }
    void TryToLoad(int poblationNum, int[] neuronalNetworkSize)
    {
        if (SaveLoad.Instance.CheckSaveData("NeuronalNetwork.json"))
        {
            string path = SaveLoad.Instance.SaveDirectory + "NeuronalNetwork.json";
            SavedGeneration saveData = JsonUtility.FromJson<SavedGeneration>(File.ReadAllText(path));
            if(Enumerable.SequenceEqual(saveData.savedLayers,neuronalNetworkSize)&&saveData.savedPopulation == poblationNum)
            {
                for (int i = 0; i < poblation.Length; i++)
                {
                    poblation[i].GetComponent<DinoBehaviour>().LoadNeuronalNetwork(neuronalNetworkSize, saveData.savedNeuronalNetworks[i]);                    
                }
            }
           
        }
        
    }    

    public bool CheckExtinction()
    {
        for (int i = 0; i < poblation.Length; i++)
        {
            if (InfoDirector.Instance.DinosAlive[i])
                return false;
        }
        return true;
    }

    public void SetSpeed(float speed)
    {
        for (int i = 0; i < poblation.Length; i++)
        {
            poblation[i].GetComponent<DinoBehaviour>().SetSpeed(speed);
        }
    }
    
    public void NewGeneration()
    {
        generation++;
        for (int i = 0; i < poblation.Length; i++)
        {
            poblation[i].GetComponent<DinoBehaviour>().CalculateFitness();
        }
        NeuronalList.Sort();
        for (int i = 0; i < poblation.Length / 2; i++)
        {
            NeuronalList[poblation.Length - 1 - i] = new NeuronalNetwork(NeuronalList[i]);
            NeuronalList[i] = new NeuronalNetwork(NeuronalList[i]);
            NeuronalList[poblation.Length - 1 - i].Mutar();
        }
        for (int i = 0; i < poblation.Length; i++)
        {
            poblation[i].GetComponent<DinoBehaviour>().Reset(NeuronalList[i], infoLenght);
        }
    }
    [System.Serializable]
    public class SavedGeneration
    {
        public NeuronalNetwork.SavedNeuronalNetwork[] savedNeuronalNetworks;
        public int[] savedLayers;
        public int savedPopulation;

        public SavedGeneration(NeuronalNetwork.SavedNeuronalNetwork[] neuronalNetworks, int[] layers, int population)
        {
            savedNeuronalNetworks = new NeuronalNetwork.SavedNeuronalNetwork[neuronalNetworks.Length];
            for (int i = 0; i < savedNeuronalNetworks.Length; i++)
            {
                savedNeuronalNetworks[i] = neuronalNetworks[i];
            }
            savedLayers = new int[layers.Length];
            for (int i = 0; i < savedLayers.Length; i++)
            {
                savedLayers[i] = layers[i];
            }
            savedPopulation = population;
        }
    }
}
