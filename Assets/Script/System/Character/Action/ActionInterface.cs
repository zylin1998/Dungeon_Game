using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public interface IHurtAction 
    {
        public abstract void Hurt(float injury); 
    }
}