using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Runtime.UI
{
    public class UI_Score : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private Sequence _popSequence;
        private void Start()
        {
            Managers.Game.OnChangeScore += OnChangeScore;
            _popSequence = DOTween.Sequence()
                .AppendCallback(()=>_text.transform.localScale = Vector3.one * 1.5f)
                    .Insert(0,_text.DOScale(1, 0.1f))
                    .SetAutoKill(false);
        }

        private void OnChangeScore(int score)
        {
            _text.text = score.ToString();
            _popSequence
                .Play()
                .Restart();
        }
    }
}