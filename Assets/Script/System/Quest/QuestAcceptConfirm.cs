using System;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    public class QuestAcceptConfirm : MonoBehaviour, IConfirmUIHandler<QuestAsset>, IPageStateHandler
    {
        [SerializeField]
        private Button accept;
        [SerializeField]
        private Button cancel;

        public Action ConfirmCallBack { get; set; }

        private void Awake()
        {
            CustomContainer.AddContent(this, "QuestAccept");

            accept.onClick.AddListener(ConfirmClick);
            cancel.onClick.AddListener(CancelClick);
        }

        private void Start()
        {
            ConfirmCallBack = delegate { QuestManager.Instance.AcceptQuest(this.Content); };

            DetailPanel = CustomContainer.GetContent<QuestAcceptDetail>("QuestAccept");

            GameManager.Instance.AddPage(this);

            UIState(false);
        }

        #region IConfirmUIHandler

        public IDetailPanelHandler<QuestAsset> DetailPanel { get; private set; }

        public Button Confirm => accept;
        public Button Cancel => cancel;

        public QuestAsset Content { get; private set; }

        public void SetMessage(QuestAsset quest) 
        {
            this.Content = quest;
            
            this.DetailPanel.SetDetail(this.Content);
        }

        public void ConfirmClick() 
        {
            ConfirmCallBack.Invoke();

            UIState(false);
        }

        public void CancelClick()
        {
            UIState(false);
        }

        public void UIState(bool state) 
        {
            this.gameObject.SetActive(state);

            this.PageState = state;

            if (!state) 
            {
                this.Content = null;
                this.DetailPanel.Clear();
            }
        }

        #endregion

        #region IPageStateHandler

        public bool PageState { get; private set; }

        #endregion

        private void OnDestroy()
        {
            CustomContainer.RemoveContent(this, "QuestAccept");
        }
    }
}