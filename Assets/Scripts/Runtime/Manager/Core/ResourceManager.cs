using System.Collections.Generic;
using Runtime.Contents;
using UnityEngine;


    public class ResourceManager
    {
        private Dictionary<int, PlayerController> characterDict = new Dictionary<int, PlayerController>();


        public void Initialize()
        {
            PlayerController[] objs = Resources.LoadAll<PlayerController>("Prefabs/Characters");
            foreach (var t in objs)
                characterDict.Add(t.ID, t);
        }

        public GameObject LoadCharacter(uint id)
        {
            Debug.Log($"Load Character ID : {id}");
            GameObject obj = null;
            if (characterDict.TryGetValue((int) id, out var player))
                return player.gameObject;
            return obj = characterDict[0].gameObject;
        }
        
        public T Load<T>(string path) where T : Object
        {
            if (typeof(T) == typeof(GameObject))
            {
                string name = path;
                int index = name.LastIndexOf('/');
                if (index >= 0)
                    name = name.Substring(index + 1);

                GameObject go = Managers.Pool.GetOriginal(name);
                if (go != null)
                    return go as T;
            }
            return Resources.Load<T>(path);
        }

        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject original = Load<GameObject>($"Prefabs/{path}");
            if (original == null)
                Debug.Log($"Fail Load Prefab [Path :{path}]");
            return Instantiate(original, parent);
        }

        public GameObject Instantiate(GameObject original, Transform parent = null)
        {
            if (original.GetComponent<Poolable>() != null)
                return Managers.Pool.Pop(original, parent).gameObject;
            
            GameObject go = Object.Instantiate(original, parent);
            go.name = original.name;
            return go;
        }
        
        public void Destroy(GameObject go)
        {
            if (go == null)
                return;

            if (go.TryGetComponent<Poolable>(out Poolable poolable))
            {
                Managers.Pool.Push(poolable);
                return;
            }
            
            Object.Destroy(go);
        }
    }
