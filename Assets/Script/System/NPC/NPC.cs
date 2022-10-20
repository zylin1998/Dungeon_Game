using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;
using DialogueSystem;

namespace RoleSystem
{
    public class NPC : MonoBehaviour, IFlipHandler, IInteractHandler
    {
        [SerializeField]
        private DialogueAsset dialogue;

        private void Awake()
        {
            InteractReset();

            Scale = transform.localScale;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) { return; }

            float side = collision.transform.position.x <= transform.position.x ? -1f : 1f;

            Flip(side);

            if (CheckInput() != 0 && !GameManager.Instance.IsPageOpen)
            {
                InteractCallBack.Invoke();
            }
        }

        #region IFlipHandler

        public Vector3 Scale { get; private set; }

        public void Flip(float flip)
        {
            var newScale = Scale;

            newScale.x *= flip;

            transform.localScale = newScale;
        }

        #endregion

        #region IInteractHandler

        public Action InteractCallBack { get; set; }

        public void InteractReset()
        {
            InteractCallBack = delegate { DialogueTrigger.Instance.TriggerDialogue(dialogue); };
        }

        #endregion

        public float CheckInput()
        {
#if UNITY_STANDALONE_WIN
            return KeyManager.GetAxis("Vertical");
#endif

#if UNITY_ANDROID
        return 0;
#endif
        }
    }
}