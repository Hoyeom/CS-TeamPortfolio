using System.Collections.Generic;
using Runtime.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;


    public class PoolManager
    {
        #region Pool

        class Pool
        {
            public GameObject Original { get; private set; }
            public Transform Root { get; set; }

            private Stack<Poolable> _poolStack = new Stack<Poolable>();

            public void Initialize(GameObject original, int count = 5)
            {
                Original = original;
                Root = new GameObject().transform;
                Root.name = $"{original.name}_Root";

                for (int i = 0; i < count; i++)
                    Push(Create());
            }

            private Poolable Create()
            {
                GameObject go = Object.Instantiate<GameObject>(Original);
                go.name = Original.name;
                return go.GetOrAddComponent<Poolable>();
            }

            public void Push(Poolable poolable)
            {
                if(poolable == null)
                    return;

                poolable.transform.parent = Root;
                poolable.gameObject.SetActive(false);
                poolable.IsUsing = false;
                
                _poolStack.Push(poolable);
            }

            public Poolable Pop(Transform parent)
            {
                Poolable poolable;

                if (_poolStack.Count > 0)
                    poolable = _poolStack.Pop();
                else
                    poolable = Create();

                poolable.gameObject.SetActive(true);
                // 현재 씬으로 이동
                poolable.transform.parent = null;
                SceneManager.MoveGameObjectToScene(poolable.gameObject, SceneManager.GetActiveScene());
                
                poolable.transform.parent = parent;
                poolable.IsUsing = true;

                return poolable;
            }
            
        }

        #endregion

        private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
        private Transform _root;

        public void Initialize()
        {
            if (_root == null)
            {
                _root = new GameObject($"@{nameof(Pool)}").transform;
                _root.parent = GameObject.Find(Managers.DEFAULT_NAME).transform;
            }
        }

        public void CreatePool(GameObject original, int count = 5)
        {
            Pool pool = new Pool();
            pool.Initialize(original, count);
            pool.Root.parent = _root;

            _pools.Add(original.name, pool);
        }

        public void Push(Poolable poolable)
        {
            string name = poolable.gameObject.name;
            if (_pools.ContainsKey(name) == false)
            {
                Object.Destroy(poolable);
                return;
            }

            _pools[name].Push(poolable);
        }

        public Poolable Pop(GameObject original, Transform parent = null)
        {
            if (_pools.ContainsKey(original.name) == false) 
                CreatePool(original);

            return _pools[original.name].Pop(parent);
        }

        public GameObject GetOriginal(string name)
        {
            if (_pools.ContainsKey(name) == false)
                return null;
            return _pools[name].Original;
        }
        
        public void Clear()
        {
            foreach (Transform child in _root)
                Object.Destroy(child.gameObject);
            _pools.Clear();
        }

    }