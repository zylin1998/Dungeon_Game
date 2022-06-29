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

        public bool attack => KeyManager.GetKeyDown(KeyState.Attack);
        public bool jump => KeyManager.GetKeyDown(KeyState.Jump);
        public bool jumpHold => KeyManager.GetKey(KeyState.Jump);
        public bool dash => KeyManager.GetKeyDown(KeyState.Dash);

#endif

#if UNITY_ANDROID

        public float horizontal => MobileInput.isDrag ? MobileInput.horizontal : 0;
        public float vertical => MobileInput.isDrag ? MobileInput.vertical : 0;

        public bool attack => MobileInput.GetKeyDown(KeyState.Attack);
        public bool jump => MobileInput.GetKeyDown(KeyState.Jump);
        public bool jumpHold => MobileInput.GetKey(KeyState.Jump);
        public bool dash => MobileInput.GetKeyDown(KeyState.Dash);

#endif
    }
}