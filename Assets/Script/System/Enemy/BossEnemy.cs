using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public abstract class BossEnemy : Enemy
    {
        [Header("���y�ɶ�")]
        [SerializeField]
        protected float standTime;
        [SerializeField]
        protected bool isStand = false;
        [Header("�ؼзP��")]
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