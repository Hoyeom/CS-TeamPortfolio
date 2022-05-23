using UnityEngine;

namespace Runtime.Scenes
{
    public class MenuScene : BaseScene
    {
        protected override void Initialize()
        {
            base.Initialize();
        }

        public override void Clear()
        {
            
        }

        public void GameStart()
        {
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }
}