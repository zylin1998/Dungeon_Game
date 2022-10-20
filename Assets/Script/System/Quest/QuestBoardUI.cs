using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;

namespace QuestSystem
{
    public class QuestBoardUI : MonoBehaviour, ISlotFieldCtrlHandler<QuestSlot, QuestAsset>
    {
        public IDetailPanelHandler<QuestAsset> DetailPanel { get; private set; }

        private void Awake()
        {
            CustomContainer.AddContent(this, "QuestBoard");
        }

        void Start()
        {
            SlotField = CustomContainer.GetContent<QuestBoardSlots>("QuestBoard");
            DetailPanel = CustomContainer.GetContent<QuestBoardDetail>("QuestBoard");

            SlotField.Initialized();
            SlotField.Slots.ForEach(SetSlot);

            UIState(false);

            QuestManager.Instance.QuestUpdateCallBack += UpdateUI;

            this.UpdateUI(true);
        }

        private void Update()
        {
            if ((SlotField as QuestBoardSlots).gameObject.activeSelf && KeyManager.GetKeyDown("Attack"))
            {
                this.UIState(false);
            }
        }

        #region ISlotFieldCtrlHandler
        
        public ISlotFieldHandler<QuestSlot> SlotField { get; private set; }
        public ISlotFieldCtrlHandler<QuestSlot, QuestAsset> Controller => this;


        public void UpdateUI(bool refresh)
        {
            if (refresh) 
            {
                var quests = QuestManager.Instance.GetQuestList("QuestBoard");

                quests.ForEach(quest => quest.QuestEndCallBack = () => QuestManager.Instance.RemoveQuest(quest));

                Controller.RefreshList(quests); 
            }

            UpdateList();
        }

        public void UpdateList()
        {
            (SlotField as QuestBoardSlots).SetCash(InventorySystem.Inventory.Instance.ItemPool.Cash);

            SlotField.Slots.ForEach(slot => 
            {
                slot.CheckSlot();

                slot.gameObject.SetActive(slot.Interact);

                if (slot.Interact) { slot.UpdateSlot(); }
            });
        }

        public void SetSlot(QuestSlot slot)
        {
            slot.Button.onClick.AddListener(() => { slot.Item?.Invoke(); });
            slot.OnSelectCallBack = () => { DetailPanel.SetDetail(slot.Item); };
            slot.OnExitCallBack = DetailPanel.Clear;
        }

        #endregion

        public void UIState(bool state) 
        {
            SlotField.UIState(state);

            if (state) { UpdateList(); }
        }

        private void OnDestroy()
        {
            CustomContainer.RemoveContent(this, "QuestBoard");
        }
    }
}