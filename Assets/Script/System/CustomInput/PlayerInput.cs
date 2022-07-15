using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomInput
{
    public class PlayerInput : MonoBehaviour
    {
#if UNITY_STANDALONE_WIN

        public float horizontal => KeyManager.GetAxis("Horizontal");
        public float vertical => KeyManager.GetAxis("Vertical");

        public bool attack => KeyManager.GetKeyDown("Attack");
        public bool jump => KeyManager.GetKeyDown("Jump");
        public bool jumpHold => KeyManager.GetKey("Jump");
        public bool dash => KeyManager.GetKeyDown("Dash");

#endif

#if UNITY_ANDROID

        public float horizontal => MobileInput.GetJoyStick("Move").Horizontal;
        public float vertical => MobileInput.GetJoyStick("Move").Vertical;

        public bool attack => MobileInput.GetButtonDown("Attack");
        public bool jump => MobileInput.GetButtonDown("Jump");
        public bool jumpHold => MobileInput.GetButton("Jump");
        public bool dash => MobileInput.GetButtonDown("Dash");

#endif
    }
}