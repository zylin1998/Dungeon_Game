using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;
using DialogueSystem;

namespace RoleSystem
{
    public class NPC : MonoBehaviour, IFlipHandler, IInteractHandler, IInteractable
    {
        [SerializeField]
        private DialogueAsset dialogue;

        private void Awake()
        {
            InteractReset();

            Scale = transform.localScale;
        }

        #region IInteractable

        [SerializeField]
        private bool instance;

        public bool Instance => this.instance;

        public void Interact() 
        {
            InteractCallBack.Invoke();
        }

        #endregion

        #region IFlipHandler

        public Vector3 Scale { get; private set; }

        public void Flip(float flip)
        {
            var newScale = Scale;

            newScale.x *= flip;

            transform.localScale = newScale;
        }

        public void LookAt(Transform transform) 
        {
            var target = transform.position.x;
            var locate = this.transform.position.x;

            var distance = target - locate;

            if (distance < 0) { this.Flip(-1); }
            if (distance > 0) { this.Flip(1); }
        }

        #endregion

        #region IInteractHandler

        public Action InteractCallBack { get; set; }

        public void InteractReset()
        {
            InteractCallBack = delegate { DialogueTrigger.Instance.TriggerDialogue(dialogue); };
        }

        #endregion
    }
}