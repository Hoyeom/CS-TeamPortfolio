using System;
using UnityEngine;

namespace Runtime.Contents
{
    public class Coin : ItemBase
    {
        [SerializeField] private int coinCount = 1;
        
        public override void PickUp()
        {
            Managers.Game.Coin += coinCount;
            Managers.Resource.Destroy(gameObject);
        }
    }
}