using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

namespace RoleSystem
{
    public class GraveRobber : NormalEnemy
    {
        protected void Awake()
        {
            health = this.GetComponent<Health>();
            animator = this.GetComponentInChildren<Animator>();
           
            scale = this.transform.localScale;
            actionDetail = this.characterDetail.ActionDetail;

            health.Production();

            var enemy = LayerMask.NameToLayer("EnemyCollider");
            var player = LayerMask.NameToLayer("PlayerCollider");

            Physics2D.IgnoreLayerCollision(enemy, enemy, true);
            Physics2D.IgnoreLayerCollision(enemy, player, true);
        }

        protected void Update()
        {
            if (uncontrollable) { return; }

            SideCheck();

            if (targetSensor.Target) { Attack(); }

            if (!targetSensor.Target) { Move(); }

            Dead();
        }

        #region 動作實作

        protected override void Move()
        {
            animator.Play($"{enemyName}_Walk");

            Flip();

            float horizontal = this.transform.localScale.x > 0 ? 1 : -1;

            this.transform.Translate(horizontal * actionDetail.WalkSpeed * Time.deltaTime, 0f, 0f);
        }

        protected override void Attack()
        {
            if (targetSensor.Target == null) 
            { 
                return; 
            }

            if (targetSensor.Distance >= 1.04f) 
            {
                Move();
            }

            if (isCoolDown) 
            {
                if (targetSensor.Distance < 1.04f)
                {
                    animator.Play($"{enemyName}_Idle");
                }

                passCoolDown += Time.deltaTime;

                if (passCoolDown >= attackCoolDown) 
                {
                    isCoolDown = false;
                }
            }

            if (!isCoolDown && targetSensor.Distance < 1.04f)
            {
                isCoolDown = true;

                passCoolDown = 0;

                animator.Play($"{enemyName}_Attack1");

                onPlayingCallBack = new OnAnimatorPlaying(Attacking);

                StartCoroutine(AnimatorPlaying("Attack1"));
            }
        }

        protected override void Dead()
        {
            if(!health.IsDead) { return; }

            isDead = true;

            attackSensorWeapon.Sensor.enabled = false;
            attackSensorBody.Sensor.enabled = false;

            animator.Play($"{enemyName}_Dead");

            if (QuestManager.Instance.EnemyKillCallBack != null) 
            {
                QuestManager.Instance.EnemyKillCallBack.Invoke(this.enemyName);
            }

            InventorySystem.Inventory.Instance.IncreaseGold(10);

            Destroy(gameObject, 2.5f);
        }

        public override void Hurt(float injury)
        {
            health.Hurt(injury);

            if (health.IsDead) 
            {
                StopAllCoroutines();

                Dead();
            }
        }

        #endregion

        #region 動作事件

        private void Attacking()
        {
            float attackTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (attackTime >= 0.505f && attackTime <= 0.666f) { attackSensorWeapon.Sensor.enabled = true; }

            else { attackSensorWeapon.Sensor.enabled = false; }
        }

        #endregion
    }
}