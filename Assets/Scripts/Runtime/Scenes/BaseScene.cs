using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Scenes
{
    public abstract class BaseScene : MonoBehaviour
    {
        public Define.Scene SceneType { get; protected set; } = Define.Scene.None;

        public void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            gameObject.name = "@Scene";
            EventSystem eventSystem = FindObjectOfType<EventSystem>();

            if (eventSystem == null)
                Managers.Resource.Instantiate("UI/EventSystem").name = $"@{nameof(EventSystem)}";
            
        }

        public abstract void Clear();
    }
}