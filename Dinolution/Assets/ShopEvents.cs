using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEvents : MonoBehaviour
{
    SaveLoad SLManager;
    private void Start()
    {
        SLManager = SaveLoad.Instance;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SLManager.ChangeScene("MainMenu");
        }
    }
    public void StartGame()
    {
        SLManager.ChangeScene("DinoLand");
    }
}
