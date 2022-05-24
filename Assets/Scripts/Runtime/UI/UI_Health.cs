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
        private void Start()
        {
            Managers.Game.Player.OnChangeHealth += OnChangeHealth;
        }

        private void OnChangeHealth(float cur, float max)
        {
            _image.fillAmount = cur / max;

            if (cur > temp)
            {
                _image.color = Color.white;
                _image.DOColor(_color, .2f).Restart();
            }

            temp = cur;
        }
    }
}