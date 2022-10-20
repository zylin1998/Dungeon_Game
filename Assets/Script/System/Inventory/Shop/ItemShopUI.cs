using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;

namespace InventorySystem
{
    #region Shop Type Enum

    [System.Serializable]
    public enum EShopType
    {
        None = 0,
        Perchase = 1,
        Sell = 2
    }

    #endregion

    public class ItemShopUI : MonoBehaviour, ISlotFieldCtrlHandler<ShopSlot, ItemPool.ItemStack>
    {
        public IDetailPanelHandler<ItemPool.ItemStack> DetailPanel { get; set; }

        private void Start()
        {
            SlotField = CustomContainer.GetContent<ShopSlotField>("Shop");
            DetailPanel = CustomContainer.GetContent<ShopDetailPanel>("Shop");

            SlotField.Initialized();
            SlotField.Slots.ForEach(slot => SetSlot(slot));
;
            Inventory.Instance.OnItemChangedCallBack += UpdateUI;

            UpdateUI(!Inventory.Instance.ItemPool.IsEmpty);
        }

        protected void Update()
        {
            if ((SlotField as ShopSlotField).gameObject.activeSelf && KeyManager.GetKeyDown("Attack"))
            {
                ShopWindowState(false, EShopType.None);
            }
        }

        public void ShopWindowState(bool state, EShopType shopType)
        {
            SlotField.UIState(state);

            if (state) { (SlotField as ICategoryHandler).SelectFirst(); }

            if (shopType != EShopType.None) { UpdateUI(state); }
        }

        #region ISlotFieldCtrlHandler

        public ISlotFieldHandler<ShopSlot> SlotField { get; private set; }
        
        public ISlotFieldCtrlHandler<ShopSlot, ItemPool.ItemStack> Controller => this;
        
        public void UpdateUI(bool refresh)
        {
            var shopType = ShopManager.Instance.ShopType;

            if (refresh && shopType != EShopType.None) 
            {
                var list = new List<ItemPool.ItemStack>();

                var itemPool = Inventory.Instance.ItemPool;
                var shopList = ShopManager.Instance.ShopList;

                if (shopType == EShopType.Sell) { list = itemPool.Holds; }

                if (shopType == EShopType.Perchase) { list = shopList.Items.ConvertAll(item => itemPool.GetItem(item)); }

                Controller.RefreshList(list);
            }

            UpdateList();
        }

        public void UpdateList()
        {
            (SlotField as ShopSlotField).SetCash(Inventory.Instance.ItemPool.Cash);

            SlotField.Slots.ForEach(slot =>
            {
                slot.CheckSlot();

                slot.SetShopType(ShopManager.Instance.ShopType);

                slot.gameObject.SetActive(slot.Interact);

                if (slot.Interact) { slot.UpdateSlot(); }
            });
        }

        public void SetSlot(ShopSlot shopSlot)
        {
            shopSlot.Button.onClick.AddListener(() => ShopManager.Instance.Trade(shopSlot.Item.Item));
            shopSlot.OnSelectCallBack = () => { DetailPanel.SetDetail(shopSlot.Item); };
            shopSlot.OnExitCallBack = DetailPanel.Clear;
        }

        #endregion
    }
}