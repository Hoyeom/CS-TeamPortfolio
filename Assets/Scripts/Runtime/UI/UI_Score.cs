using System;
using DG.Tweening;
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
            _text.text = score.ToString();
            _text.transform.localScale = Vector3.one * 1.5f;
            _text.DOScale(1, 0.1f);
        }
    }
}