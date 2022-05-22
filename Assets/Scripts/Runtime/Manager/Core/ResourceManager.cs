using UnityEngine;


    public class ResourceManager
    {
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
