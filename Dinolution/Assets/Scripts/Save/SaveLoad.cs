using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;


public class SaveLoad : MonoBehaviour {

    string currentDirectory;
    public string SaveDirectory { get { return currentDirectory; } }
    static private SaveLoad instance = null;
    static public SaveLoad Instance { get { return instance; } }
    public delegate void SaveDelegate();
    public static event SaveDelegate BeforeClosing;
    private void Awake()
    {        
        if (instance == null)
        {
            instance = this;
            CheckSaveFolder();
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }        
    }

    private void CheckSaveFolder()
    {
        string path = Application.persistentDataPath;
        path += ("/Resources/Saves");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path); 
        }
        currentDirectory = path + "/";
    }
    public bool CheckSaveData(string fileName)
    {
        string path = (currentDirectory + fileName);
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }
    public void DeleteSave()
    {
        string path = (currentDirectory + "Data.json");
        File.Delete(path);
        path = (currentDirectory + "NeuronalNetwork.json");
        File.Delete(path);
    }
    public void ChangeScene(string sceneName)
    {
        if (BeforeClosing != null)
        {
            BeforeClosing();
            CleanDelegate();
        }
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public void QuitApplication()
    {
        if (BeforeClosing!=null)
        {
            BeforeClosing();
            CleanDelegate();
        }
        Application.Quit();
    }

    private void CleanDelegate()
    {
        Delegate[] functions = BeforeClosing.GetInvocationList();
        for (int i = 0; i < functions.Length; i++)
        {
            BeforeClosing -= (SaveDelegate)functions[i];
        }
    }
}
