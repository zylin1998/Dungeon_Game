using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestAcceptUI : MonoBehaviour, IConfirmUICtrlHandler<QuestAsset>
    {
        private void Awake()
        {
            CustomContainer.AddContent(this, "QuestAccept");
        }

        #region IConfirmUICtrlHandler

        public IConfirmUIHandler<QuestAsset> ConfirmHandler { get; private set; }

        private void Start()
        {
            ConfirmHandler = CustomContainer.GetContent<QuestAcceptConfirm>("QuestAccept");
        }

        public void SendMessage(QuestAsset message) 
        {
            this.ConfirmHandler.SetMessage(message);

            this.ConfirmHandler.UIState(true);
        }

        public void SendMessage(QuestAsset message, Action confirm)
        {
            this.ConfirmHandler.ConfirmCallBack = confirm;

            SendMessage(message);
        }

        #endregion

        private void OnDestroy()
        {
            CustomContainer.RemoveContent(this, "QuestAccept");
        }
    }
}