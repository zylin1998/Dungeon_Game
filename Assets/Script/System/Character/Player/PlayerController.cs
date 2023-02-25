using System.Collections;
using UnityEngine;
using CustomInput;

namespace RoleSystem
{
    public abstract class PlayerController : MonoBehaviour, IHurtAction, IGroundCheck, IWallCheck, IFlipHandler
    {
        [SerializeField]
        private CharacterDetailAsset characterDetail;
        /// <summary>
        /// Require Components
        /// </summary>
        protected Animator animator;
        protected Collider2D capsule;
        protected Rigidbody2D rigid;
        /// <summary>
        /// Detail Script
        /// </summary>
        protected Health health;
        protected ActionDetail actionDetail;
        protected AttackSensor attackSensor;
        protected InteractSensor interactSensor;

        protected int jumpCount;
        protected float jumpPress = 0f;

        protected float dashPress = 0f;

        public bool IsDead { get; protected set; }
        public bool Uncontrollable { get; protected set; }
        public bool HasDetail => this.characterDetail != null;


        protected virtual void Awake()
        {
            health = this.GetComponent<Health>();
            rigid = this.GetComponent<Rigidbody2D>();
            capsule = this.GetComponent<Collider2D>();
            animator = this.GetComponentInChildren<Animator>();
            attackSensor = this.GetComponentInChildren<AttackSensor>();
            interactSensor = this.GetComponentInChildren<InteractSensor>();

            actionDetail = characterDetail.ActionDetail;

            health.SingleOnly();

            PlayerInform.Instance.SetLife(health.NormalizedLife);
            PlayerInform.Instance.SetMP(health.NormalizedMP);
        }

        #region 動作抽象

        protected abstract void Move(Vector2 value);
        
        protected abstract void Jump(bool value);

        protected abstract void Dash(bool value);

        protected abstract void Attack(bool value);

        public abstract void Hurt(float injury);

        protected abstract void Dead();

        #endregion

        #region 動作事件觸發

        protected System.Action OnPlayingCallBack;

        protected virtual IEnumerator AnimatorPlaying(string name, bool uncontrollable)
        {
            this.Uncontrollable = uncontrollable;

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(name)) { yield return null; }

            while (animator.GetCurrentAnimatorStateInfo(0).IsName(name))
            {
                OnPlayingCallBack.Invoke();

                yield return null;
            }

            this.Uncontrollable = false;
        }

        #endregion

        #region IWallCheck

        public float IsCollision { get; set; }

        public virtual float CheckSpeed(float speed)
        {
            if (this.IsCollision > 0 && speed < 0) { return 0f; }

            if (this.IsCollision < 0 && speed > 0) { return 0f; }

            return speed;
        }

        #endregion

        #region IFlipHandler

        public Vector3 Scale { get; protected set; }

        protected bool isFlip;
        public bool IsFlip { get => isFlip; set => isFlip = value; }
        
        public void Flip(float flip)
        {
            if (flip == 0) { return; }

            this.IsFlip = flip < 0;

            transform.localScale = new Vector3(flip, 1f, 1f);
        }

        public void LookAt(Transform transform) 
        {

        }

        #endregion

        #region IGroundCheck

        public bool IsGround { get; set; }
        
        public float VerticalVelocity => rigid.velocity.y;

        public virtual void Land() 
        {
            this.jumpCount = 0;
        }

        #endregion
    }
}