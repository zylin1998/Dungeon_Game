using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;

namespace RoleSystem
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class SwordMan : PlayerController, IInputClient
    {
        #region IInputClient

        [SerializeField]
        private bool isCurrent;
        [SerializeField]
        private List<IInputClient.RequireAxes> axes;
        
        public List<IInputClient.RequireAxes> Axes => this.axes;

        public bool IsCurrent 
        { 
            get 
            {
                return this.isCurrent;
            } 

            set 
            {
                this.isCurrent = value;

                if (!value) { Move(Vector2.zero); }
            }
        }

        public void GetValue(IEnumerable<AxesValue<float>> values) 
        {
            if (this.Uncontrollable) { return; }

            var direct = new Vector2();

            values.ToList().ForEach(v => 
            {
                if (v.AxesName == "Horizontal") { direct.x = v.Value; return; }

                if (v.AxesName == "Vertical") { direct.y = v.Value; return; }
            });
            
            this.Move(direct);
        }

        public void GetValue(IEnumerable<AxesValue<bool>> values)
        {
            values.ToList().ForEach(v => 
            {
                if (v.AxesName == "Jump") { this.Jump(v.Value); return; }

                if (v.AxesName == "Dash") { this.Dash(v.Value); return; }

                if (v.AxesName == "Attack") { this.Attack(v.Value); return; }

                if (v.AxesName == "Inventory" && v.Value) { CustomContainer.GetContent<InventorySystem.InventoryUI>("Inventory")?.UIState(true); }
            });
        }

        #endregion

        protected override void Awake()
        {
            base.Awake();
        }

        protected void Start()
        {
            KeyManager.SetBasic(this, true);

            Initialize();
        }

        #region 動作實作

        protected override void Move(Vector2 value)
        {
            var horizontal = value.x;
            var vertival = value.y;

            interactSensor.SetDirect(value);

            if (horizontal == 0) { if (this.IsGround) animator.Play("Idle"); }

            if (horizontal != 0)
            {
                this.Flip(horizontal * -1);

                float speed = horizontal * actionDetail.WalkSpeed * Time.deltaTime;

                transform.transform.Translate(CheckSpeed(speed), 0f, 0f);

                if (this.IsGround) { animator.Play("Run"); }
            }
        }

        private bool jumpState = false;

        protected override void Jump(bool jump)
        {
            if (jump)
            {
                if (jumpState && jumpPress <= actionDetail.MaxJumpHold)
                {
                    jumpPress += Time.deltaTime;

                    if (jumpPress >= 0.1f) { rigid.AddForce(rigid.velocity = Vector2.up * actionDetail.JumpForce * 0.55f); }
                }

                if (!jumpState && jumpCount < actionDetail.MaxJumpCount )
                {
                    if (jumpCount > 0 && !this.IsGround) { jumpCount++; }

                    if (jumpCount == 0 && this.IsGround) { jumpCount = 1; }

                    if (jumpCount == 0 && !this.IsGround) { jumpCount = 2; }

                    jumpPress = 0f;

                    rigid.AddForce(rigid.velocity = Vector2.up * actionDetail.JumpForce * 0.4f);

                    jumpState = true;
                }
            }

            if (!jump || Uncontrollable) { jumpState = false; }

            if (this.VerticalVelocity != 0 && !Uncontrollable) { animator.Play("Jump"); }
        }

        private bool dashState = false;

        protected override void Dash(bool dash)
        {
            if (dashState) 
            {
                dashPress += Time.deltaTime;

                if (dashPress >= actionDetail.DashCoolDown) { dashState = false; }
            }

            if (dash && !dashState)
            {
                dashPress = 0f;

                animator.Play("Dash");

                this.OnPlayingCallBack = Dashing;
                StartCoroutine(AnimatorPlaying("Dash", true));

                dashState = true;
            }
        }

        protected override void Attack(bool attack)
        {
            if (attack)
            {
                animator.Play("Attack");

                this.OnPlayingCallBack = Attacking;
                StartCoroutine(AnimatorPlaying("Attack", true)); ;
            }
        }

        protected override void Dead()
        {
            if(!health.IsDead) { return; }

            this.IsDead = true;

            animator.Play("Die");
        }

        public override void Hurt(float injury)
        {
            health.Hurt(injury);

            PlayerInform.Instance.SetLife(health.NormalizedLife);

            if (this.health.IsDead) { Dead(); }
        }

        #endregion

        #region 動作事件

        protected void Dashing()
        {
            float dashTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float speed = (isFlip ? 1 : -1) * actionDetail.DashSpeed * Time.deltaTime;

            if (dashTime >= 0.2f && dashTime <= 0.8f)
            {
                rigid.velocity = Vector2.zero;
                transform.transform.Translate(CheckSpeed(speed), 0f, 0f);
            }
        }

        protected void Attacking()
        {
            float attackTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (attackTime >= 0.505f && attackTime <= 0.666f) { attackSensor.Sensor.enabled = true; }

            else { attackSensor.Sensor.enabled = false; }
        }

        #endregion

        #region Initialize

        private void Initialize() 
        {
            var player = LayerMask.NameToLayer("PlayerCollider");
            var enemy = LayerMask.NameToLayer("EnemyCollider");

            Physics2D.IgnoreLayerCollision(player, enemy, true);
        }

        #endregion

        public void OnDestroy()
        {
            if (KeyManager.HasInputClient) { KeyManager.SetBasic(this, false); }
        }
    }
}