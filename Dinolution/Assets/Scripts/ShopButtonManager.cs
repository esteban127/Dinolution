using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonManager : MonoBehaviour
{
    [SerializeField] int[] prices = null;
    public int ActualPrice { get { return prices[actualLevel]; } }
    int actualLevel;

    public void ActualizateButton(int level, int totalgold)
    {
        if (level < prices.Length)
        {
            actualLevel = level;
            GetComponentInChildren<Text>().text = prices[level].ToString();
            GetComponent<Button>().interactable = prices[level] <= totalgold;
        }
        else
        {
            GetComponentInChildren<Text>().text = "Max level";
            GetComponent<Button>().interactable = false;
        }

    }
}
