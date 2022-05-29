using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class UI_Background : MonoBehaviour
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private Image _nextImage;
        [SerializeField] private Image _prevImage;
        [SerializeField] private float fadeDuration = 1;
        private int _length;
        private int _prevCount;
        private int _nextCount = 0;

        private int NextCount
        {
            get => _nextCount;
            set
            { 
                _prevCount = _nextCount;
                if (value == 0)
                    _prevCount = _length -1;
                _nextCount = value;
                if (_nextCount % _length == 0)
                    _nextCount = 0;
            }
        }

        private Sequence fadeInOut;
        
        private void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;

            _length = _sprites.Length;
            NextCount = 1;

            Managers.Game.OnNextArea += NextImage;

            fadeInOut = DOTween
                .Sequence()
                .Pause()
                .AppendCallback(() =>
                {
                    _prevImage.sprite = _sprites[_prevCount];
                    _nextImage.sprite = _sprites[_nextCount];

                    _prevImage.color = Color.white;
                    _nextImage.color = new Color(1, 1, 1, 0);
                })
                .Insert(0,
                    _prevImage.DOFade(0, fadeDuration))
                .Insert(0,
                    _nextImage.DOFade(1, fadeDuration))
                .OnComplete(() => NextCount++)
                .SetAutoKill(false);

        }

        private void NextImage()
        {
            fadeInOut.Play();
        }
        
    }
}