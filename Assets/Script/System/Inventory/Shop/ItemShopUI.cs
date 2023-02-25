using System.Linq;
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

    public class ItemShopUI : MonoBehaviour, ISlotFieldCtrlHandler<ItemPool.ItemStack>, IInputClient
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

        #region ISlotFieldCtrlHandler

        public ISlotFieldHandler<ItemPool.ItemStack> SlotField { get; private set; }
        
        public ISlotFieldCtrlHandler<ItemPool.ItemStack> Controller => this;
        
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

            if (refresh) 
            {
                var navi = SlotField.Content.GetComponent<INavigationCtrl>();

                navi.GetSelectables(n => (n as ISlotHandler<ItemPool.ItemStack>).Interact);
                navi.SetNavigation();
            }
        }

        public void UpdateList()
        {
            (SlotField as ShopSlotField).SetCash(Inventory.Instance.ItemPool.Cash);

            SlotField.Slots.ForEach(slot =>
            {
                if (slot is ShopSlot shopSlot)
                {
                    shopSlot.SetShopType(ShopManager.Instance.ShopType);

                    shopSlot.UIState(shopSlot.Interact);

                    if (shopSlot.Interact) { shopSlot.UpdateSlot(); }
                }
            });
        }

        public void SetSlot(ISlotHandler<ItemPool.ItemStack> shopSlot)
        {
            shopSlot.Button.onClick.AddListener(() => ShopManager.Instance.Trade(shopSlot.Content.Item));
            shopSlot.OnSelectCallBack = () => { DetailPanel.SetDetail(shopSlot.Content); };
            shopSlot.OnExitCallBack = DetailPanel.Clear;
        }

        public void UIState(bool state) 
        {
            if (state) { UpdateUI(state); }

            this.SlotField.UIState(state);

            KeyManager.SetCurrent(this, state);
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
                if (v.AxesName == "Attack" && v.Value) { this.UIState(false); }
            });
        }

        #endregion
    }
}