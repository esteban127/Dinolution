using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [SerializeField] Text maxtext = null;
    [SerializeField] Slider slider = null;
    [SerializeField] DinoStatsManager stats = null;

    private void Start()
    {
        if (stats.SpeedLevel == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            maxtext.text = "x" + (1 + stats.SpeedLevel * 0.5).ToString("F1");
        }
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
    }

    private void SwipeDetector_OnSwipe(SwipeData data)
    {
        if(data.Direction == SwipeDirection.Right|| data.Direction == SwipeDirection.Left)
        {
            slider.value += ((data.StartPosition.x - data.EndPosition.x) * 0.01f);
        }
       
    }
}
