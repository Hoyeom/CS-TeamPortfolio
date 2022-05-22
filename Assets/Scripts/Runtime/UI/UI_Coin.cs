﻿using TMPro;
using UnityEngine;

namespace Runtime.UI
{
    public class UI_Coin : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private void Start()
        {
            Managers.Game.OnChangeCoin += OnChangeCoin;
        }

        private void OnChangeCoin(int score)
        {
            _text.text = $"Coin : {score.ToString()}";
        }
    }
}