using UnityEngine;

namespace Runtime.Utils
{
    public static class Extension
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
            => Util.GetOrAddComponent<T>(go);
    }
}