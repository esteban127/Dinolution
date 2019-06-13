using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoGenerator : MonoBehaviour
{
    //[SerializeField] int poblationNum = 10;
    List<NeuronalNetwork> NeuronalList;
    public int generation = 0;
    //[SerializeField] int[] neuronalNetworkSize = null;    
    //[SerializeField] float generationLifeSpan = 5;
    [SerializeField] GameObject dinoPrefab = null;
    [SerializeField] GameObject dinoStyle = null;
    GameObject[] poblation;
    int infoLenght = 1;
    

    /*void Start()
    {
        poblation = new GameObject[poblationNum];
        NeuronalList = new List<NeuronalNetwork>();
        for (int i = 0; i < poblationNum; i++)
        {
            GameObject myDino = Instantiate(dinoPrefab);
            NeuronalNetwork myIA = new NeuronalNetwork(neuronalNetworkSize);
            NeuronalList.Add(myIA);
            myDino.GetComponent<DinoBehaviour>().Reset(myIA, neuronalNetworkSize);
            myDino.transform.position = transform.position;

            GameObject style = Instantiate(dinoStyle);
            myDino.GetComponent<DinoBehaviour>().Rendering = true; //render all test
            style.transform.SetParent(myDino.transform);

            poblation[i] = myDino;
            myDino.transform.SetParent(transform);
        }
        /*poblation[0].name = "Pro";
        poblation[0].GetComponent<DinoBehaviour>().AlphaBoy = true; // render only 1
        GameObject style = Instantiate(dinoStyle);
        style.transform.SetParent(poblation[0].transform);
        
    }*/
    public void Initalzie(int poblationNum, int[] neuronalNetworkSize)
    {
        infoLenght = neuronalNetworkSize[0];
        poblation = new GameObject[poblationNum];
        NeuronalList = new List<NeuronalNetwork>();
        for (int i = 0; i < poblationNum; i++)
        {
            GameObject myDino = Instantiate(dinoPrefab);
            NeuronalNetwork myIA = new NeuronalNetwork(neuronalNetworkSize);
            NeuronalList.Add(myIA);
            myDino.GetComponent<DinoBehaviour>().Reset(myIA, infoLenght);
            myDino.transform.position = transform.position;
            GameObject style = Instantiate(dinoStyle);
            myDino.GetComponent<DinoBehaviour>().Rendering = true; //render all test
            style.transform.SetParent(myDino.transform);

            poblation[i] = myDino;
            myDino.transform.SetParent(transform);
        }
    }
    

    public bool CheckExtinction()
    {
        for (int i = 0; i < poblation.Length; i++)
        {
            if (poblation[i].activeInHierarchy)
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
}
