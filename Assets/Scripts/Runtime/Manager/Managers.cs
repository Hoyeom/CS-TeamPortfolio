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
    
    private readonly PoolManager _pool = new PoolManager();
    private readonly SceneManagerEx _scene = new SceneManagerEx();
    private readonly GameManager _game = new GameManager();
    private readonly ResourceManager _resource = new ResourceManager();
    private readonly SoundManager _sound = new SoundManager();
    private readonly DataManager _data = new DataManager();

    public static PoolManager Pool => Instance._pool;
    public static SceneManagerEx Scene => Instance._scene;
    public static GameManager Game => Instance._game;
    public static ResourceManager Resource => Instance._resource;
    public static SoundManager Sound => Instance._sound;
    public static DataManager Data => Instance._data;
    
    public const string DEFAULT_NAME = "@Managers";
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

        _instance._data.Initialize();
        _instance._game.Initialize();
        _instance._resource.Initialize();
    }

    public static void Clear()
    {
        Scene.Clear();
        Game.Clear();
    }
}