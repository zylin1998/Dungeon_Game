using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;

namespace RoleSystem
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(PlayerInput))]
    public class SwordMan : PlayerController
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected void Start()
        {
            Initialize();
        }

        protected void Update()
        {
            if (!hasDetail) { return; }

            if (uncontrollable) { return; }

            Move();

            Jump();

            Dash();

            Attack();

            Dead();
        }

        #region 動作實作

        protected override void Move()
        {
            float horizontal = playerInput.horizontal;
            float vertical = playerInput.vertical;

            if (horizontal == 0 || pause)
            {
                if (isGround) { animator.Play("Idle"); }

                if (pause) { return; }
            }

            if (horizontal > 0) { isFlip = true; }
            if (horizontal < 0) { isFlip = false; }

            Flip(isFlip);

            float speed = horizontal * actionDetail.WalkSpeed * Time.deltaTime;

            transform.transform.Translate(CheckSpeed(speed), 0f, 0f);

            if (horizontal != 0 && isGround) { animator.Play("Run"); }
        }

        protected override void Jump()
        {
            if (pause) { return; }

            if (isGround) { jumpCount = 0; }

            if (!isGround && jumpCount > actionDetail.MaxJumpCount) { return; }

            if (playerInput.jump)
            {
                animator.Play("Jump");

                isGround = false;

                jumpCount++;

                if (jumpCount <= actionDetail.MaxJumpCount)
                {
                    jumpPress = 0f;

                    rigid.AddForce(rigid.velocity = Vector2.up * actionDetail.JumpForce * 0.4f);
                }

                return;
            }

            if (!isGround && playerInput.jumpHold && jumpPress <= actionDetail.MaxJumpHold)
            {
                jumpPress += Time.deltaTime;

                if (jumpPress >= 0.1f) { rigid.AddForce(rigid.velocity = Vector2.up * actionDetail.JumpForce * 0.55f); }
            }
        }

        protected override void Dash()
        {
            if (pause) { return; }

            var passTime = Time.realtimeSinceStartup - dashPress;

            if (dashPress != 0 && passTime < actionDetail.DashCoolDown) { return; }

            if (playerInput.dash)
            {
                dashPress = Time.realtimeSinceStartup;

                animator.Play("Dash");

                onPlayingCallBack = new OnAnimatorPlaying(Dashing);
                StartCoroutine(AnimatorPlaying("Dash"));
            }
        }

        protected override void Attack()
        {
            if (playerInput.attack)
            {
                animator.Play("Attack");

                onPlayingCallBack = new OnAnimatorPlaying(Attacking);
                StartCoroutine(AnimatorPlaying("Attack"));
            }
        }

        protected override void Dead()
        {
            if(!health.dead) { return; }

            dead = true;

            animator.Play("Die");
        }

        public override void Hurt(float injury)
        {
            health.Hurt(injury);
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

            if (attackTime >= 0.505f && attackTime <= 0.666f) { attackSenesor.Sensor.enabled = true; }

            else { attackSenesor.Sensor.enabled = false; }
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
    }
}