using System;
using System.Collections.Generic;
using Runtime.Contents;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GameManager
{
    public PlayerController Player { get; private set; }
    public int CharacterID { get; set; }

    private GameSetting _setting = null;
    
    private Vector3 _spawnPos = Vector3.zero;
    private Define.Dir _spawnDir = Define.Dir.Left;
    
    private int floorCount = 0; // TEST
    private int score = 0;
    
    public int Score
    {
        get => score;
        set
        {
            score = value;
            OnChangeScore?.Invoke(score);
        }
    }

    public event Action<int> OnChangeScore;
    public event Action<int> OnChangeCoin;

    private int coin = 0;
    
    

    public int Coin
    {
        get => coin;
        set
        {
            coin = value;
            OnChangeCoin?.Invoke(coin);
        }
    }

    public GameSetting Setting => _setting;

    private Queue<Platform> PlatformQueue { get; } = new Queue<Platform>();
    private Queue<GameObject> destroyQueue { get; } = new Queue<GameObject>();
    
    private int bestScore = 0;
    public int BestScore
    {
        get=> (score > bestScore) ? bestScore = score : bestScore;
        set => bestScore = value;
    }

    public void Initialize()
    {
        _setting = Managers.Resource.Load<GameSetting>("Settings/Normal");
        _setting = Managers.Game.Setting;
    }

    public void GameStart()
    {
        while (PlatformQueue.Count < 20)
            GeneratePlatform();
    }

    public void SpawnPlayer()
    {
        GameObject obj = Object.Instantiate(Managers.Resource.LoadCharacter(CharacterID));
        obj.transform.position = Vector3.zero;
        Player = obj.GetComponent<PlayerController>();
    }

    public Platform GetNextPlatform()
    {
        if (PlatformQueue.Count < 20)
            GeneratePlatform();
        if (destroyQueue.Count > 10)
        {
            Managers.Resource.Destroy(destroyQueue.Dequeue());
        }

        Platform platform = PlatformQueue.Dequeue();
        destroyQueue.Enqueue(platform.gameObject);
        return platform;
    }

    public void Clear()
    {
        PlatformQueue.Clear();
    }

    private void GeneratePlatform()
    {
        int dir = _spawnDir == Define.Dir.Left ? -1 : 1;

        // Range는 Max 미표함이므로 +1
        int rand = Random.Range(_setting.MinPlatform, _setting.MaxPlatform + 1);

        for (int i = 0; i < rand; i++)
        {
            _spawnPos += new Vector3(_setting.PlatformOffsetX * dir, _setting.PlatformOffsetY);
            GameObject obj = Managers.Resource.Instantiate("Platform");
            obj.transform.position = _spawnPos;
            
            Platform platform = obj.GetComponent<Platform>();
            platform.Dir = _spawnDir;

            // TODO 아이템 랜덤 생성
            
            SpawnItem(floorCount, platform.transform.position);
            
            //
            floorCount++;

            PlatformQueue.Enqueue(platform);
        }

        _spawnDir = _spawnDir == Define.Dir.Left ? Define.Dir.Right : Define.Dir.Left;
    }

    private int itemCount = 0;
    private List<int> randValues = new List<int>();
    
    private void SpawnItem(int floor, Vector3 pos)
    {
        if (floor < 100) return;

        GameObject obj = null;
        
        if (floor > 300)
        {
            if (floor % 10 == 0)
            {
                obj = Managers.Resource.Instantiate("Items/HealthPotion");
                obj.transform.position = pos + Vector3.up * .5f;
            }
        }
        else
        {
            int temp = floor switch
            {
                >= 250 => 3,
                >= 200 => 2,
                _ => 1
            };
            
            if (itemCount <= 0)
            {
                randValues.Clear();
                for (int i = 0; i < temp; i++)
                {
                    int rand = Random.Range(0, 50);
                    if (randValues.Contains(rand))
                    {
                        i--;
                        continue;
                    }
                    randValues.Add(rand);
                }
                itemCount = 50;
            }

            foreach (int value in randValues)
            {
                if (value == itemCount)
                {
                    obj = Managers.Resource.Instantiate("Items/HealthPotion");
                    obj.transform.position = pos + Vector3.up * .5f;
                    break;
                }
            }
            
            itemCount--;
        }
        

        if(obj != null) return;
        
        SpawnCoin(pos);
    }

    void SpawnCoin(Vector3 pos)
    {
        GameObject obj = null;
        float rand = Random.Range(0, 100f);
        
        Debug.Log("코인 생성");
        
        if(rand < 4)
            obj = Managers.Resource.Instantiate("Items/CoinSet");
        else if(rand < 25) // 1/5
            obj = Managers.Resource.Instantiate("Items/Coin");
        if(obj != null)
            obj.transform.position = pos + Vector3.up * .5f;
    }

    public Vector3 GetNextPos(Vector3 pos,Platform platform,Define.Dir dir)
    {
        int[] index = {-1, 1};
        return platform.Dir != dir ?
            pos + new Vector3(_setting.PlatformOffsetX * index[(int) dir], _setting.PlatformOffsetY) :
            platform.transform.position;
    }
}