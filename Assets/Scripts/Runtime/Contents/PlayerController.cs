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
        private Controller _controller;

        public event Action OnDied;
        [SerializeField] private float jumpPower = 1;
        [SerializeField] private float jumpDuration = 0.1f;

        [SerializeField] private float maxHealth = 100;
        public float MaxHealth => maxHealth;
        private float curHealth;

        enum State
        {
            Idle,
            Moving,
            Die
        }

        [SerializeField] private State _state = State.Idle;
        
        public float Health
        {
            get => curHealth;
            set
            {
                if(_state == State.Die) return;

                curHealth = Mathf.Clamp(value, 0, MaxHealth);
                
                OnChangeHealth?.Invoke(curHealth, maxHealth);
                
                if (curHealth <= 0)
                    OnDied?.Invoke();
            }
        }

        [SerializeField] private int id = 0;
        private SpriteRenderer _renderer;
        private Animator _anim;
        private static readonly int Jump = Animator.StringToHash("jump");
        public int ID => id;

        public event Action<float, float> OnChangeHealth;
        
        
        private void OnEnable()
        {
            _state = State.Idle;
            if (_controller == null)
            {
                _controller = new Controller();
                _controller.Enable();
                _controller.Player.Jump.started += JumpStarted;
                _controller.Player.Turn.performed += TurnPerformed;
            }
        }

        private void OnDisable()
        {
            if (_controller != null)
            {
                _controller.Disable();
                _controller = null;
            }
        }
        
        private void Start()
        {
            _anim = GetComponent<Animator>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _setting = Managers.Game.Setting;
            Health = maxHealth;

            OnDied += PlayerDie;
        }



        public event Action OnGameOver;

        private void PlayerDie()
        {
            // 이건... 너무 막짯는데?...// 
            _state = State.Die;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOJump(transform.position, 1, 1, .4f))
                .Insert(0.4f, transform.DOMove(transform.position + Vector3.down * 10, 1))
                .Insert(0.4f, transform.DOScale(0, 1f))
                .OnComplete(() =>
                {
                    Managers.Resource.Instantiate("UI/GameOverUI");
                    OnGameOver?.Invoke();
                });
        }

        private void TurnPerformed(InputAction.CallbackContext context)
        {
            bool flip = context.ReadValue<float>() < 0;
            _dir = flip ? Define.Dir.Left : Define.Dir.Right;
            _renderer.flipX = flip;
        }

        private void JumpStarted(InputAction.CallbackContext context)
        {
            if (_state == State.Idle)
                _state = State.Moving;
            if(_state == State.Die)
                return;

            Platform platform = Managers.Game.GetNextPlatform();
            
            if (platform.Dir != _dir)
                _state = State.Die;
            else
                Health += MaxHealth * .1f;

            _anim.SetBool(Jump, true);
            transform.DOJump(Managers.Game.GetNextPos(transform.position,platform,_dir),
                    jumpPower,
                    1,
                    jumpDuration)
                .OnComplete(() =>
                {
                    _anim.SetBool(Jump,false);
                    if (_state != State.Die)
                    {
                        Managers.Game.Score++;
                    }
                    else
                        OnDied?.Invoke();
                });
        }
        
        

        private void Update()
        {
            if (_state != State.Moving) return;
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
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position,_dir == Define.Dir.Left ? -transform.right : transform.right);
        }
    }
}