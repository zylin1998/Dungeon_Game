using System.Collections;
using UnityEngine;
using CustomInput;

namespace RoleSystem
{
    public abstract class PlayerController : MonoBehaviour, IHurtAction
    {
        [SerializeField]
        private CharacterDetailAsset characterDetail;

        protected int jumpCount;
        protected float jumpPress = 0f;

        protected float dashPress = 0f;

        protected bool isFlip;
        protected bool isGround;
        protected float isCollision;

        protected Health health;
        protected PlayerInput playerInput;
        protected ActionDetail actionDetail;

        protected Animator animator;
        protected Rigidbody2D rigid;
        protected AttackSensor attackSenesor;
        protected Collider2D capsule;

        public float VerticalVelocity => rigid.velocity.y;
        public bool IsFlip { get => isFlip; set => isFlip = value; }
        public bool IsGround { get => isGround; set => isGround = value; }
        public float IsCollision { get => isCollision; set => isCollision = value; }

        protected bool pause => GameManager.Instance.pause;
        protected bool hasDetail => actionDetail != null;
        protected bool dead;

        #region 確認是否可控

        protected bool uncontrollable
        {
            get
            {
                foreach (string action in actionDetail.UnControllableAction)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName(action)) { return true; }
                }

                if (dead) { return true; }

                return false;
            }
        }

        #endregion

        protected virtual void Awake()
        {
            health = this.GetComponent<Health>();
            rigid = this.GetComponent<Rigidbody2D>();
            playerInput = this.GetComponent<PlayerInput>();
            capsule = this.GetComponent<Collider2D>();
            animator = this.GetComponentInChildren<Animator>();
            attackSenesor = this.GetComponentInChildren<AttackSensor>();

            actionDetail = characterDetail.ActionDetail;

            health.SingleOnly();

            PlayerInform.Instance.SetLife(health.NormalizedLife);
            PlayerInform.Instance.SetMP(health.NormalizedMP);
        }

        #region 動作抽象

        protected abstract void Move();
        
        protected abstract void Jump();

        protected abstract void Dash();

        protected abstract void Attack();

        public abstract void Hurt(float injury);

        protected abstract void Dead();

        #endregion

        #region 動作事件觸發

        protected delegate void OnAnimatorPlaying();
        protected OnAnimatorPlaying onPlayingCallBack;

        protected IEnumerator AnimatorPlaying(string name)
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(name)) { yield return null; }

            while (animator.GetCurrentAnimatorStateInfo(0).IsName(name))
            {
                onPlayingCallBack.Invoke();

                yield return null;
            }
        }

        #endregion

        protected float CheckSpeed(float speed)
        {
            if (isCollision > 0 && speed < 0) { return 0f; }

            if (isCollision < 0 && speed > 0) { return 0f; }

            return speed;
        }

        public void Flip(bool flip)
        {
            var flipX = flip ? -1 : 1;

            transform.localScale = new Vector3(flipX, 1f, 1f);
        }
    }
}