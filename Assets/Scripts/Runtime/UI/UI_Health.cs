using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class UI_Health : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        private void Start()
        {
            Managers.Game.Player.OnChangeHealth += OnChangeHealth;
        }

        private void OnChangeHealth(float cur, float max)
        {
            _image.fillAmount = cur / max;
        }
    }
}