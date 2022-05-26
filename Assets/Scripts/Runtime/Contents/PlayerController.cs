using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Runtime.Contents
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int HashJump = Animator.StringToHash("jump");
        private static readonly int Die = Animator.StringToHash("die");
        
        private Define.Dir _dir = Define.Dir.Left;
        private GameSetting _setting;
        private Controller _controller;

        [SerializeField] private float jumpPower = 1;
        [SerializeField] private float jumpDuration = 0.1f;
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private State _state = State.Idle;

        public float MaxHealth => maxHealth;
        
        private float curHealth;

        enum State
        {
            Idle,
            Moving,
            Die
        }

        public float Health
        {
            get => curHealth;
            set
            {
                curHealth = Mathf.Clamp(value, 0, MaxHealth);
                
                OnChangeHealth?.Invoke(curHealth, maxHealth);

                if (curHealth <= 0 && _state != State.Die) 
                {
                    Debug.Log("Die");
                    _controller.Disable();
                    OnDied?.Invoke();
                }
            }
        }

        [SerializeField] private int id = 0;
        public int ID => id;
        
        private SpriteRenderer _renderer;
        private Animator _anim;


        public event Action<float, float> OnChangeHealth;
        public event Action OnGameOver;
        public event Action OnDied;
        
        
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

        /// <summary>
        /// 플레이어 사망
        /// </summary>
        private void PlayerDie()
        {
            _state = State.Die;

            Managers.Sound.Play("Fx/Died");
            
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOJump(transform.position, 1, 1, .4f)
                    .OnComplete(()=>_anim.SetBool(Die,true)))
                .Insert(0.4f, transform.DOMove(transform.position + Vector3.down * 10, 2f))
                .Insert(0.4f, transform.DOScale(0, 2f))
                .OnComplete(() => OnGameOver?.Invoke());
        }

        /// <summary>
        /// 좌 우 회전
        /// </summary>
        /// <param name="context">float</param>
        private void TurnPerformed(InputAction.CallbackContext context)
        {
            bool flip = context.ReadValue<float>() < 0;
            _dir = flip ? Define.Dir.Left : Define.Dir.Right;
            _renderer.flipX = flip;
        }

        /// <summary>
        /// 현재 방향으로 점프
        /// </summary>
        /// <param name="context">bool</param>
        private void JumpStarted(InputAction.CallbackContext context)
        {
            if (_state == State.Idle)
                _state = State.Moving;
            Jump(Managers.Game.GetNextPlatform());
        }
        
        /// <summary>
        /// 현재 방향에 플랫폼이 있다면 점수
        /// 없다면 사망
        /// </summary>
        /// <param name="platform"></param>
        private void Jump(Platform platform)
        {
            bool isDead = platform.Dir != _dir;
            
            Health += MaxHealth * .1f;
            
            _anim.SetBool(HashJump, true);
            transform.DOJump(Managers.Game.GetNextPos(transform.position,platform,_dir),
                    jumpPower,
                    1,
                    jumpDuration)
                .OnComplete(() =>
                {
                    _anim.SetBool(HashJump,false);
                    if (isDead) Health -= MaxHealth;
                    else Managers.Game.Score++;
                });
                
        }


        /// <summary>
        /// 움직이는 상태라면 체력 감소
        /// </summary>
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