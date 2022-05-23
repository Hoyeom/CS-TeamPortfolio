using System;
using TMPro;
using UnityEngine;

namespace Runtime.UI
{
    public class UI_Coin : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private void Awake()
        {
            Managers.Game.OnChangeCoin += OnChangeCoin;
        }

        private void Start()
        {
            OnChangeCoin(Managers.Game.Coin);
        }

        private void OnChangeCoin(int score)
        {
            _text.text = $"{score.ToString()}";
        }
    }
}