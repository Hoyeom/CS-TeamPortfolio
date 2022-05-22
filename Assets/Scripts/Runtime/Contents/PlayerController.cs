using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Runtime.Contents
{
    public class PlayerController : MonoBehaviour
    {
        private Define.Dir _dir = Define.Dir.Left;
        private GameSetting _setting;
        
        
        public event Action OnDied;
        [SerializeField] private float jumpPower = 1;
        [SerializeField] private float jumpDuration = 0.1f;

        [SerializeField] private float maxHealth = 100;
        public float MaxHealth => maxHealth;
        private float curHealth;

        enum State
        {
            Idle,
            StartGame,
            Die
        }

        [SerializeField] private State _state = State.Idle;
        
        public float Health
        {
            get => curHealth;
            set
            {
                curHealth = Mathf.Clamp(value, 0, MaxHealth);
                OnChangeHealth?.Invoke(curHealth, maxHealth);
                if (curHealth <= 0)
                {
                    _state = State.Die;
                    OnDied?.Invoke();
                }
            }
        }

        public event Action<float, float> OnChangeHealth;
        
        
        private void Start()
        {
            _setting = Managers.Game.Setting;
            Health = maxHealth;
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Idle:
                    IdleUpdate();
                    break;
                case State.StartGame:
                    StartUpdate();
                    break;
                case State.Die:
                    DiedUpdate();
                    break;
            }
        }

        private void IdleUpdate()
        {
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                Move();
                _state = State.StartGame;
            }
        }
        private void StartUpdate()
        {
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
                Move();
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
                Turn(Define.Dir.Right);
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
                Turn(Define.Dir.Left);
            

            HealthDown();
        }

        private void HealthDown()
        {
            float minus = 0;
            if (Managers.Game.Score >= 300) minus = .5f;
            else if (Managers.Game.Score >= 250) minus = .4f;
            else if (Managers.Game.Score >= 200) minus = .3f;
            else if (Managers.Game.Score >= 150) minus = .25f;
            else if (Managers.Game.Score >= 100) minus = .2f;
            else minus = .1f;
            
            Health -= MaxHealth * minus * Time.deltaTime;
        }
        
        private void DiedUpdate()
        {
            DOTween.Clear();
            Managers.Scene.LoadScene(Define.Scene.Game);
        }

        private void Move()
        {
            Platform platform = Managers.Game.GetNextPlatform();
            if (platform.Dir != _dir)
                _state = State.Die;
            else
                Health += MaxHealth * .1f;
            
            // transform.position += Managers.Game.GetNextPos(_dir);

            transform.DOJump(transform.position +
                             Managers.Game.GetNextPos(_dir),
                    jumpPower,
                    1,
                    jumpDuration)
                .OnComplete(() =>
                {
                    if (_state != State.Die)
                        Managers.Game.Score++;
                    else
                        OnDied?.Invoke();
                });
        }

        private void Turn(Define.Dir dir)
        {
            _dir = dir;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position,_dir == Define.Dir.Left ? -transform.right : transform.right);
        }
    }
}