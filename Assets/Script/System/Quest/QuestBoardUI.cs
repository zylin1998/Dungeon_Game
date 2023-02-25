using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;

namespace QuestSystem
{
    public class QuestBoardUI : MonoBehaviour, ISlotFieldCtrlHandler<QuestAsset>, IInputClient
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
        
        public ISlotFieldHandler<QuestAsset> SlotField { get; private set; }
        public ISlotFieldCtrlHandler<QuestAsset> Controller => this;


        public void UpdateUI(bool refresh)
        {
            if (refresh) 
            {
                var quests = QuestManager.Instance.GetQuestList("QuestBoard");

                quests.ForEach(quest => quest.QuestEndCallBack = () => QuestManager.Instance.RemoveQuest(quest));

                Controller.RefreshList(quests);
            }

            UpdateList();
            
            if (refresh)
            {
                var navi = SlotField.Content.GetComponent<INavigationCtrl>();

                navi.GetSelectables(n => (n as ISlotHandler<QuestAsset>).Interact);
                navi.SetNavigation();
            }
        }

        public void UpdateList()
        {
            (SlotField as QuestBoardSlots).SetCash(InventorySystem.Inventory.Instance.ItemPool.Cash);

            SlotField.Slots.ForEach(slot => 
            {
                slot.UIState(slot.Interact);

                if (slot.Interact) { slot.UpdateSlot(); }
            });
        }

        public void SetSlot(ISlotHandler<QuestAsset> slot)
        {
            slot.Button.onClick.AddListener(() => { slot.Content?.Invoke(); });
            slot.OnSelectCallBack = () => { DetailPanel.SetDetail(slot.Content); };
            slot.OnExitCallBack = DetailPanel.Clear;
        }

        public void UIState(bool state) 
        {
            SlotField.UIState(state);

            KeyManager.SetCurrent(this, state);

            if (state) { UpdateList(); }
        }

        #endregion

        #region IInputClient

        [SerializeField]
        private List<IInputClient.RequireAxes> axes;

        public List<IInputClient.RequireAxes> Axes => axes;

        public bool IsCurrent { get; set; }

        private float holdFast = 0.1f;
        private float holdSlow = 0.5f;
        private float holding = 0f;

        private int count = 0;

        public void GetValue(IEnumerable<AxesValue<float>> values)
        {
            var direct = Vector2.zero;

            values.ToList().ForEach(v =>
            {
                if (v.AxesName == "Horizontal" && v.Value != 0) { direct.x = v.Value; }

                if (v.AxesName == "Vertical" && v.Value != 0) { direct.y = v.Value; }
            });

            if (this.SlotField is INaviPanelCtrl ctrl)
            {
                if (holding == 0f)
                {
                    ctrl.SelectPanel(direct);

                    count++;
                }

                holding += Time.deltaTime;
            }

            if (holding >= (count <= 2 ? holdSlow : holdFast) || direct == Vector2.zero)
            {
                holding = 0;

                if (direct == Vector2.zero) { count = 0; }
            }
        }

        public void GetValue(IEnumerable<AxesValue<bool>> values)
        {
            values.ToList().ForEach(v =>
            {
                if (v.AxesName == "Inventory" && v.Value) { this.UIState(false); }

                if (v.AxesName == "Attack" && v.Value) { this.UIState(false); }
            });
        }

        #endregion

        private void OnDestroy()
        {
            CustomContainer.RemoveContent(this, "QuestBoard");
        }
    }
}