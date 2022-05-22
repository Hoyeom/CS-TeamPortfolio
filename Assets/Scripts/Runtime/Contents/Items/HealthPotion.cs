using UnityEngine;

namespace Runtime.Contents
{
    public class HealthPotion : ItemBase
    {
        public override void PickUp()
        {
            Managers.Game.Player.Health = Managers.Game.Player.MaxHealth;
            Managers.Resource.Destroy(gameObject);
        }
    }
}