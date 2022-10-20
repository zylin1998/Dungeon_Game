using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;

namespace InventorySystem
{
    public class InventoryUI : MonoBehaviour, ISlotFieldCtrlHandler<DetailSlot, ItemPool.ItemStack>
    {
        public IDetailPanelHandler<ItemPool.ItemStack> DetailPanel { get; private set; }

        private Inventory Inventory => Inventory.Instance;

        #region Script Behaviour

        private void Start()
        {
            SlotField = CustomContainer.GetContent<DetailSlotField>("Inventory");
            DetailPanel = CustomContainer.GetContent<ItemDetailPanel>("Inventory");

            SlotField.Initialized();
            SlotField.Slots.ForEach(slot => SetSlot(slot));
            
            Inventory.OnItemChangedCallBack += UpdateUI;

            UpdateUI(!Inventory.ItemPool.IsEmpty);
        }

        private void Update()
        {
#if UNITY_STANDALONE_WIN
            if(KeyManager.GetKeyDown("Inventory")) 
            {
                SlotField.UIState(!(SlotField as DetailSlotField).gameObject.activeSelf);

                (SlotField as ICategoryHandler).SelectFirst();
            }
#endif
        }

        #endregion

        #region Slot Function

        public void ItemUsed(DetailSlot slot) 
        {
            if (Inventory.ItemPool.Category.Compare(slot.Item.Item.Category, "Jewelry")) { return; }

            slot.Item.Item.Used();
        }

        #endregion

        #region ISlotFieldCtrlHandler

        public ISlotFieldHandler<DetailSlot> SlotField { get; private set; }

        public ISlotFieldCtrlHandler<DetailSlot, ItemPool.ItemStack> Controller => this;

        public void UpdateUI(bool refresh)
        {
            if (refresh) { Controller.RefreshList(Inventory.ItemPool.Holds); }

            UpdateList();
        }

        public void UpdateList() 
        {
            (SlotField as DetailSlotField).SetCash(Inventory.ItemPool.Cash);

            SlotField.Slots.ForEach(slot =>
            {
                slot.CheckSlot();

                slot.gameObject.SetActive(slot.Interact);

                if (slot.Interact) { slot.UpdateSlot(); }
            });
        }

        public void SetSlot(DetailSlot slot)
        {
            slot.Button.onClick.AddListener(() => { ItemUsed(slot); });
            slot.OnSelectCallBack = () => { DetailPanel.SetDetail(slot.Item); };
            slot.OnExitCallBack = DetailPanel.Clear;
        }

        #endregion
    }
}