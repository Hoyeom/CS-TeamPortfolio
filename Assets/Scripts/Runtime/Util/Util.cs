using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Runtime.Util
{
    public class Util
    {
        public static T FindOrNewComponent<T>(string name) where T : Component
        {
            T component = GameObject.Find(name).GetComponent<T>();
            
            if (component == null) 
                component = new GameObject(name).AddComponent<T>();
            
            return component;
        }
    }
}