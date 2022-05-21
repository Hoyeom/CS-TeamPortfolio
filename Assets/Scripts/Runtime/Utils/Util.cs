using UnityEngine;

namespace Runtime.Utils
{
    public class Util
    {
        public static T FindOrNewComponent<T>(out T component, string name) where T : Component
        {
            component = GameObject.Find(name).GetComponent<T>();
            
            if (component == null) 
                component = new GameObject(name).AddComponent<T>();
            
            return component;
        }
    }
}