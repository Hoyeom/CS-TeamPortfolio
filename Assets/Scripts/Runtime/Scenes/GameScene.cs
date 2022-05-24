﻿using System.Collections.Generic;
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
            Managers.Game.Player.OnGameOver += GameOverUI;
        }

        private void GameOverUI()
        {
            Managers.Resource.Instantiate("UI/GameOverUI");
        }

        public override void Clear()
        {
            
        }
    }
}