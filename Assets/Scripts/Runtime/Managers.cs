using System.Collections;
using System.Collections.Generic;
using Runtime.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    private static Managers instance;

    private static Managers Instance
    {
        get
        {
            Initialize();
            return instance;
        }
    }

    private const string DEFAULT_NAME = "@Managers";

    private GameManager _game;
    public static GameManager Game => Instance._game;

    private static void Initialize()
    {
        if (instance != null) return;

        instance = GameObject.Find(DEFAULT_NAME).GetComponent<Managers>();
        
        if (instance == null) 
            instance = new GameObject(DEFAULT_NAME).AddComponent<Managers>();

        Scene managerScene = SceneManager.CreateScene("Managers");

        SceneManager.MoveGameObjectToScene(instance.gameObject, managerScene);
        
    }
}