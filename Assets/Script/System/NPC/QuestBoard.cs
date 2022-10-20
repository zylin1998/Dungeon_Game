using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using CustomInput;

namespace RoleSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class QuestBoard : MonoBehaviour, IInteractHandler
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

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) { return; }

            if (CheckInput() && !GameManager.Instance.IsPageOpen)
            {
                InteractCallBack.Invoke();
            }
        }

        public bool CheckInput()
        {
#if UNITY_STANDALONE_WIN
            return KeyManager.GetAxis("Vertical") != 0;
#endif

#if UNITY_ANDROID
        return 0;
#endif
        }
    }
}