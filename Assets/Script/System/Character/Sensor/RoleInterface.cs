using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public interface IGroundCheck
    {
        public bool IsGround { get; set; }

        public float VerticalVelocity { get; }

        public void Land();
    }

    public interface IWallCheck
    {
        public float IsCollision { get; set; }

        public float CheckSpeed(float speed);
    }

    public interface ITargetCheck 
    {
        public Transform Target { get; }

        public void SetTarget(Transform target);
    }
}