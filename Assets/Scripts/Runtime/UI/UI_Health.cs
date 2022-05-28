using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class UI_Health : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _color;
        private float temp = float.MaxValue;
        private Sequence _popColor;
        
        private void Start()
        {
            Managers.Game.Player.OnChangeHealth += OnChangeHealth;
            _popColor = DOTween.Sequence()
                .AppendCallback(()=>_image.color = Color.white)
                .Insert(0,_image.DOColor(_color, 0.2f))
                .SetAutoKill(false);
        }

        private void OnChangeHealth(float cur, float max)
        {
            _image.fillAmount = cur / max;

            if (cur > temp)
            {
                _popColor.Play().Restart();
            }

            temp = cur;
        }
    }
}