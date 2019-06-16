using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSText : MonoBehaviour
{   
    float updateTime = 0.5f;
    float intervalTime = 0;
    float frameCount = 0;
    void Update()
    {
        intervalTime += Time.deltaTime;
        frameCount++;
        if(intervalTime>= updateTime)
        {
            GetComponent<Text>().text = ("FPS: " + frameCount);
            frameCount = 0;
            intervalTime = 0;
        }
    }
}
