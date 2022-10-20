using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

namespace RoleSystem
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class DragonWorrior : BossEnemy, IGroundCheck, IWallCheck
    {
        [Header("技能生成物件")]
        [SerializeField]
        private GameObject fireBall;
        [SerializeField]
        private GameObject iblast;
        [SerializeField]
        private GameObject explosion;
        [Header("技能生成區域")]
        [SerializeField]
        private Transform magicPoint;
        [SerializeField]
        private Transform magicParent;
        [Header("重傷暈眩")]
        [SerializeField]
        private bool isDizzy;
        [SerializeField]
        private float dizzyTime;
        [SerializeField]
        private float[] dizzyRates;
        [SerializeField]
        private float dizzyRate;
        private Queue<float> dizzyQueue;
        
        private Rigidbody2D rigid;

        public bool IsGround { get; set; }
        public float IsCollision { get; set; }
        public float VerticalVelocity => rigid.velocity.y;

        protected void Awake()
        {
            health = this.GetComponent<Health>();
            rigid = this.GetComponent<Rigidbody2D>();
            animator = this.GetComponentInChildren<Animator>();

            scale = this.transform.localScale;
            actionDetail = this.characterDetail.ActionDetail;

            health.Production();

            dizzyQueue = new Queue<float>(dizzyRates);
            dizzyRate = dizzyQueue.Dequeue();

            var enemy = LayerMask.NameToLayer("EnemyCollider");
            var player = LayerMask.NameToLayer("PlayerCollider");

            Physics2D.IgnoreLayerCollision(enemy, enemy, true);
            Physics2D.IgnoreLayerCollision(enemy, player, true);
        }

        protected void Update()
        {
            if (isDead) { return; }

            if (!Target) 
            {
                animator.Play("Idle");
                
                return;
            }

            if (!isDizzy) { IsStand(); }

            if (!isStand) { Move(); }
        }

        #region 基礎動作

        protected override void Move()
        {
            if (isActive) { return; }

            isActive = true;

            var rdm = Random.Range(1, 100);

            if (rdm <= 40f) { Jump(); }

            if (rdm >= 41f) { Attack(); }
        }

        protected void Jump() 
        {
            if (IsGround)
            {
                var rdm = Random.Range(1, 100);
                var distance = Target.transform.position.x - this.transform.position.x;
                var force = rdm <= 41 ? Vector2.up : new Vector2(distance * 0.08f, 1f);
                
                animator.Play("Jump");
                rigid.AddForce(rigid.velocity = force * actionDetail.JumpForce * 0.4f);

                if (rdm <= 41) 
                {
                    StartCoroutine(FlyKick());
                }
            }
        }

        protected override void Attack()
        {
            var rdm = Random.Range(1, 100);

            if (rdm <= 40f) 
            {
                animator.Play("Strike");
                onPlayingCallBack = Strike;
                StartCoroutine(AnimatorPlaying("Strike"));
            }

            if (rdm >= 71f && rdm <= 100f) 
            { 
                animator.Play("Crouch");
                onPlayingCallBack = FireBall;
                StartCoroutine(AnimatorPlaying("Crouch_ATK"));
            }

            if (rdm >= 41f && rdm <= 70f) 
            { 
                animator.Play("Attack");
                onPlayingCallBack = Iblast;
                StartCoroutine(AnimatorPlaying("Attack"));
            }
        }

        public override void Hurt(float injury) 
        {
            if (isDead) { return; }

            health.Hurt(injury);

            if (health.NormalizedLife <= dizzyRate) 
            {
                StartCoroutine(Dizzy());

                if (dizzyQueue.Count == 0) { dizzyRate = -1; }

                if (dizzyQueue.Count > 0) { dizzyRate = dizzyQueue.Dequeue(); }

                return;
            }
            
            if (isDizzy) 
            {
                StopCoroutine(Dizzy());

                isStand = true;
                isActive = false;
                isDizzy = false;
            }

            if (health.IsDead)
            {
                StopAllCoroutines();

                Dead();
            }
        }
        
        protected override void Dead()
        {
            if (!health.IsDead) { return; }

            isStand = false;
            isActive = false;
            isDizzy = false;

            isDead = true;

            animator.Play("Die");

            QuestManager.Instance.EnemyKilled(this.enemyName);

            InventorySystem.Inventory.Instance.IncreaseGold(150);

            Destroy(gameObject, 1f);
        }

        #endregion
        
        #region 動畫觸發事件
        /// <summary>
        /// Crouch 事件
        /// </summary>
        private void FireBall() 
        {
            float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            
            if (time >= 0.666f)
            {
                var fireball = Instantiate(this.fireBall, magicPoint.position, magicPoint.rotation, magicParent);

                fireball.transform.localScale = this.transform.localScale * 1.5f;
                fireball.GetComponent<Magic>().SetDamage(health.CurrentDamage);
                
                onPlayingCallBack = delegate { };
            }
        }
        /// <summary>
        /// Iblast 事件
        /// </summary>
        private void Iblast() 
        {
            float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (time >= 0.666f)
            {
                var iblast = Instantiate(this.iblast, magicPoint.position, magicPoint.rotation, magicParent);

                iblast.transform.localScale = this.transform.localScale * 1.5f;
                iblast.GetComponent<Magic>().SetDamage(health.CurrentDamage);

                onPlayingCallBack = delegate { };
            }
        }
        /// <summary>
        /// Strike 事件
        /// </summary>
        private void Strike() 
        {
            float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float speed = (isFlip ? 1 : -1) * actionDetail.DashSpeed * Time.deltaTime;

            if (time >= 0.3f)
            {
                rigid.velocity = Vector2.zero;
                transform.transform.Translate(CheckSpeed(speed), 0f, 0f);
            }
        }
        /// <summary>
        /// FlyKick 事件
        /// </summary>
        /// <returns></returns>
        private IEnumerator FlyKick() 
        {
            yield return new WaitForSeconds(0.2f);

            var animation = "FlyKick";

            animator.Play(animation);

            onPlayingCallBack = delegate
            {
                float kickTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                float speed = (isFlip ? 1 : -1) * actionDetail.DashSpeed * Time.deltaTime;

                if (kickTime >= 0.222f)
                {
                    rigid.velocity = Vector2.zero;
                    transform.transform.Translate(CheckSpeed(speed), 0f, 0f);
                }
            };

            yield return base.AnimatorPlaying(animation);

            onPlayingCallBack = delegate { };
        }
        /// <summary>
        /// Dizzy 事件
        /// </summary>
        /// <returns></returns>
        private IEnumerator Dizzy() 
        {
            isStand = false;
            isActive = true;
            isDizzy = true;

            animator.Play("Dizzy");

            yield return new WaitForSeconds(dizzyTime);

            isStand = true;
            isActive = false;
            isDizzy = false;
        }
        /// <summary>
        /// 動畫事件處理
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override IEnumerator AnimatorPlaying(string name)
        {
            yield return base.AnimatorPlaying(name);

            isActive = false;
            isStand = true;
        }

        #endregion

        private float passTime = 0f;

        private void IsStand()
        {
            if (!isStand) { return; }

            if (isStand) 
            {
                animator.Play("Idle");
            
                passTime += Time.deltaTime; 
            }

            if (passTime >= standTime) 
            {
                isStand = false;

                passTime = 0f;
            }

            isFlip = Target.position.x >= this.transform.position.x;
            
            Flip();
        }

        public void Land() 
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) { return; }

            rigid.velocity = Vector2.zero;

            isStand = true;
            isActive = false;
        }

        public virtual float CheckSpeed(float speed)
        {
            if (IsCollision > 0 && speed < 0) { return 0f; }

            if (IsCollision < 0 && speed > 0) { return 0f; }

            return speed;
        }
    }
}