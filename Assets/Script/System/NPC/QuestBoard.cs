using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using CustomInput;

namespace RoleSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class QuestBoard : MonoBehaviour, IInteractHandler, IInteractable
    {
        private QuestBoardUI questBoard { get; set; }

        private void Awake()
        {
            InteractReset();
        }

        private void Start()
        {
            questBoard = CustomContainer.GetContent<QuestBoardUI>("QuestBoard");
        }

        #region IInteractHandler

        public Action InteractCallBack { get; set; }

        public void InteractReset()
        {
            InteractCallBack = delegate { this.questBoard.UIState(true); };
        }

        #endregion

        #region IInteractable

        [SerializeField]
        private bool instance;

        public bool Instance => this.instance;

        public void Interact()
        {
            if (!GameManager.Instance.IsPageOpen)
            {
                InteractCallBack.Invoke();
            }
        }

        #endregion
    }
}