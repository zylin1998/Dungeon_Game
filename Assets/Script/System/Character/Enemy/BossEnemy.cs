using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public abstract class BossEnemy : Enemy, ITargetCheck
    {
        [Header("°±¹y®É¶¡")]
        [SerializeField]
        protected float standTime;
        [SerializeField]
        protected bool isStand = false;
        [SerializeField]
        protected bool isActive = false;
        
        public Transform Target { get; protected set; }

        public bool passStandTime { get; private set; }

        protected void SideCheck()
        {
            if (Target)
            {
                isFlip = Target.position.x > this.transform.position.x;
                Flip();
            }
        }

        public void SetTarget(Transform target) 
        {
            this.Target = target;
        }
    }
}