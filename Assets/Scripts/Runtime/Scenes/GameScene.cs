using System.Collections.Generic;
using Runtime.Contents;
using UnityEngine;

namespace Runtime.Scenes
{
    public class GameScene : BaseScene
    {

        
        protected override void Initialize()
        {
            base.Initialize();
            Managers.Game.GameStart();
            Managers.Game.SpawnPlayer();
        }

        public override void Clear()
        {
            
        }
    }
}