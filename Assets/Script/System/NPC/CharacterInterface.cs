using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public interface IFlipHandler
    {
        public Vector3 Scale { get; }

        public void Flip(bool flip)
        {
            var f = flip ? 1f : -1f;

            Flip(f);
        }

        public void Flip(float flip);

        public void LookAt(Transform transform);
    }

    public interface IInteractHandler 
    {
        public Action InteractCallBack { get; set; }

        public void InteractReset();
    }
}