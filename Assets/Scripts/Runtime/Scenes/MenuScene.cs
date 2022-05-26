using UnityEngine;

namespace Runtime.Scenes
{
    public class MenuScene : BaseScene
    {
        protected override void Initialize()
        {
            base.Initialize();
            Managers.Sound.Play("Bgm/Main",Define.Sound.Bgm);
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