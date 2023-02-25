using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;

namespace InventorySystem
{
    public class InventoryUI : MonoBehaviour, ISlotFieldCtrlHandler<ItemPool.ItemStack>, IInputClient
    {
        public IDetailPanelHandler<ItemPool.ItemStack> DetailPanel { get; private set; }

        private Inventory Inventory => Inventory.Instance;

        #region Script Behaviour

        private void Awake()
        {
            CustomContainer.AddContent(this, "Inventory");
        }

        private void Start()
        {
            SlotField = CustomContainer.GetContent<DetailSlotField>("Inventory");
            DetailPanel = CustomContainer.GetContent<ItemDetailPanel>("Inventory");

            SlotField.Initialized();
            SlotField.Slots.ForEach(slot => SetSlot(slot));
            
            Inventory.OnItemChangedCallBack += UpdateUI;

            UpdateUI(!Inventory.ItemPool.IsEmpty);
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

        #region ISlotFieldCtrlHandler

        public ISlotFieldHandler<ItemPool.ItemStack> SlotField { get; private set; }

        public ISlotFieldCtrlHandler<ItemPool.ItemStack> Controller => this;

        public void UpdateUI(bool refresh)
        {
            if (refresh) { Controller.RefreshList(Inventory.ItemPool.Holds); }

            UpdateList();

            if (refresh)
            {
                var navi = SlotField.Content.GetComponent<INavigationCtrl>();

                navi.GetSelectables(n => (n as ISlotHandler<ItemPool.ItemStack>).Interact);
                navi.SetNavigation();
            }
        }

        public void UpdateList() 
        {
            (SlotField as DetailSlotField).SetCash(Inventory.ItemPool.Cash);

            SlotField.Slots.ForEach(slot =>
            {
                slot.UIState(slot.Interact);

                if (slot.Interact) { slot.UpdateSlot(); }
            });
        }

        public void SetSlot(ISlotHandler<ItemPool.ItemStack> slot)
        {
            slot.Button.onClick.AddListener(() => { ItemUsed(slot as DetailSlot); });
            slot.OnSelectCallBack = () => { DetailPanel.SetDetail(slot.Content); };
            slot.OnExitCallBack = DetailPanel.Clear;
        }

        #region Slot Function

        public void ItemUsed(DetailSlot slot) 
        {
            if (Inventory.ItemPool.Category.Compare(slot.Content.Item.Category, "Jewelry")) { return; }

            slot.Content.Item.Used();
        }

        #endregion

        public void UIState(bool state) 
        {
            this.SlotField.UIState(state);

            KeyManager.SetCurrent(this, state);
        }

        #endregion
    }
}