using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{ 
    public abstract class NormalEnemy : Enemy
    {
        [Header("攻擊重置係數")]
        [SerializeField]
        protected float attackCoolDown;
        [SerializeField]
        protected bool isCoolDown = false;
        [SerializeField]
        protected float passCoolDown;
        [Header("感測器腳本")]
        [SerializeField]
        protected TargetSensor targetSensor;
        [SerializeField]
        protected AttackSensor attackSensorWeapon;
        [SerializeField]
        protected AttackSensor attackSensorBody;

        protected void SideCheck()
        {
            if (targetSensor.Target != null)
            {
                isFlip = targetSensor.Target.transform.position.x > transform.position.x;
                Flip();
            }
        }
    }
}