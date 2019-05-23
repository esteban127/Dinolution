using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoGenerator : MonoBehaviour
{
    [SerializeField] int poblationNum = 10;
    List<NeuronalNetwork> NeuronalList;
    public int generation = 0;
    [SerializeField] int[] neuronalNetworkSize = null;    
    [SerializeField] float generationLifeSpan = 5;
    [SerializeField] GameObject dinoPrefab = null;
    [SerializeField] GameObject dinoStyle = null;
    GameObject[] poblation;
    float counter = 0;

    void Start()
    {
        poblation = new GameObject[poblationNum];
        NeuronalList = new List<NeuronalNetwork>();
        for (int i = 0; i < poblationNum; i++)
        {

            GameObject myDino = Instantiate(dinoPrefab);
            NeuronalNetwork myIA = new NeuronalNetwork(neuronalNetworkSize);
            NeuronalList.Add(myIA);
            myDino.GetComponent<DinoBehaviour>().ReciveNeuronalNetwork(myIA);
            myDino.transform.position = transform.position;
            poblation[i] = myDino;
            myDino.transform.SetParent(transform);
        }
        poblation[0].name = "Pro";
        GameObject style = Instantiate(dinoStyle);
        style.transform.SetParent(poblation[0].transform);

    }

    private void Update()
    {
        if (counter > generationLifeSpan || checkExtinction())
        {
            NewGeneration();
            counter = 0;
        }
        InfoDirector.Instance.GenerationLifetime = counter;
        counter += Time.deltaTime;
    }

    private bool checkExtinction()
    {
        for (int i = 0; i < poblation.Length; i++)
        {
            if(poblation[i].activeInHierarchy)
                return false;
        }
        return true;
    }

    private void NewGeneration()
    {
        generation++;
        ObstaclesGenerator.Instance.Reset();
        for (int i = 0; i < poblationNum; i++)
        {
            poblation[i].GetComponent<DinoBehaviour>().CalculateFitness();
        }
        NeuronalList.Sort();
        for (int i = 0; i < poblationNum / 2; i++)
        {
            NeuronalList[poblationNum - 1 - i] = new NeuronalNetwork(NeuronalList[i]);
            NeuronalList[i] = new NeuronalNetwork(NeuronalList[i]);
            NeuronalList[poblationNum - 1 - i].Mutar();

        }
        for (int i = 0; i < poblationNum; i++)
        {
            poblation[i].GetComponent<DinoBehaviour>().ReciveNeuronalNetwork(NeuronalList[i]);            
        }
    }
}
