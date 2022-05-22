using System;
using TMPro;
using UnityEngine;

namespace Runtime.UI
{
    public class UI_Score : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private void Start()
        {
            Managers.Game.OnChangeScore += OnChangeScore;
        }

        private void OnChangeScore(int score)
        {
            _text.text = $"Score : {score.ToString()}";
        }
    }
}