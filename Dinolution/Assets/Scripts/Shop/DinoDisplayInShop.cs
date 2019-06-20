using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DinoDisplayInShop : MonoBehaviour
{
    [SerializeField] Sprite[] dinos = null;

    public void selectDino(int stage)
    {
        GetComponent<Image>().sprite = dinos[stage];
    }
}
