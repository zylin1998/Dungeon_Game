using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public abstract class BossEnemy : Enemy
    {
        [Header("停頓時間")]
        [SerializeField]
        protected float standTime;
        [SerializeField]
        protected bool isStand = false;
        [Header("目標感測")]
        [SerializeField]
        protected Transform target;

        public bool passStandTime { get; private set; }

        protected void SideCheck()
        {
            if (target)
            {
                isFlip = target.position.x > this.transform.position.x;
                Flip();
            }
        }
    }
}