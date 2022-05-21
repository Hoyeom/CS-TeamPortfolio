using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    private static Managers _instance;

    private static Managers Instance
    {
        get
        {
            Initialize();
            return _instance;
        }
    }

    private const string DEFAULT_NAME = "@Managers";

    private readonly GameManager _game = new GameManager();
    public static GameManager Game => Instance._game;

    private void Awake()
        => gameObject.name = DEFAULT_NAME;

    private void Start()
        => Initialize();

    
    
    
    private static void Initialize()
    {
        if (_instance != null) return;

        Util.FindOrNewComponent(out _instance, DEFAULT_NAME);

        Scene managerScene = SceneManager.CreateScene("Managers");

        SceneManager.MoveGameObjectToScene(_instance.gameObject, managerScene);
    }
}