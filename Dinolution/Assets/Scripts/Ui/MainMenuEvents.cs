using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuEvents : MonoBehaviour
{
    [SerializeField] ConfirmPanel confirm = null;
    [SerializeField] Button continueButton = null;
    SaveLoad SLManager;    
    private void Start()
    {
        SLManager = SaveLoad.Instance;
        if (SLManager.CheckSaveData("Data.json"))
        {
            continueButton.interactable = true;
        }
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SLManager.QuitApplication();
        }
    }

    public void NewGame()
    {
        if (SLManager.CheckSaveData("Data.json"))
        {
            StartCoroutine(WaitForConfirm());
        }
        else
        {
            SLManager.ChangeScene("Shop");
        }
    }
    public void Continue()
    {        
        SLManager.ChangeScene("Shop");        
    }


    IEnumerator WaitForConfirm()
    {
        confirm.Ask();
        while (confirm.ConfirmResult == "Null")
        {
            yield return null;
        }
        if (confirm.ConfirmResult == "Yes")
        {
            SLManager.DeleteSave();
            SLManager.ChangeScene("Shop");
        }
    }
}
