using UnityEngine;

namespace Runtime.Contents
{
    [CreateAssetMenu(fileName = "NewGameSetting", menuName = "GameSetting", order = 0)]
    public class GameSetting : ScriptableObject
    {
        [SerializeField] private int minPlatform = 1;
        [SerializeField] private int maxPlatform = 8;
        [SerializeField] private float platformOffsetX = 1;
        [SerializeField] private float platformOffsetY = 0.2f;

        public int MinPlatform => minPlatform;
        public int MaxPlatform => maxPlatform;
        public float PlatformOffsetX => platformOffsetX;
        public float PlatformOffsetY => platformOffsetY;
    }
}