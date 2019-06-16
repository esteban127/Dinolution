using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObstacleBehaviour : MonoBehaviour
{
    bool[] destroyedStatus;
    [SerializeField] Sprite[] destructionSprites;
    int currentSprite = 0;

    public bool CheckDestroyed(int dinoID)
    {
        return destroyedStatus[dinoID];
    }
    public void Destroy(int dinoID)
    {
        destroyedStatus[dinoID]=true;
        SetDestroyedSprite();
    }

    private void SetDestroyedSprite()
    {
        if (compareDestroyedStatus())
        {
            swapSprite(2);
        }
        else
        {
            swapSprite(1);
        }
    }

    private bool compareDestroyedStatus()
    {
        bool[] dinosAlive = InfoDirector.Instance.DinosAlive;
        for (int i = 0; i < destroyedStatus.Length; i++)
        {
            if(destroyedStatus[i]!= dinosAlive[i])
            {
                return false;
            }
        }
        return true;
    }

    public void ResetStatus(int population)
    {
        swapSprite(0);
        destroyedStatus = new bool[population];
        for (int i = 0; i < destroyedStatus.Length; i++)
        {
            destroyedStatus[i] = false;
        }
    }

    private void swapSprite(int spriteID)
    {
        if (currentSprite != spriteID)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = destructionSprites[spriteID];
            currentSprite = spriteID;
        }
    }
}
