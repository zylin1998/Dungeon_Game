using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomInput;

namespace InventorySystem
{
    public class ItemShopUI : MonoBehaviour, ICategoryHandler
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

        [SerializeField]
        private ShopList shopList;
        [SerializeField]
        private EShopType shopType;
        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private Transform itemShopUI;
        [SerializeField]
        private Transform perchaseParent;
        [SerializeField]
        private Transform sellParent;
        [SerializeField]
        private Text shopTitle;
        [SerializeField]
        private Text itemDetail;
        [SerializeField]
        private Text itemSell;
        [SerializeField]
        private Text itemPerchase;
        [SerializeField]
        private Text itemCash;
        [SerializeField]
        private GameObject slotPrefab;
        [SerializeField]
        private CategorySelection[] selections;
        [SerializeField]
        private List<ShopSlot> shopSlots;
        [SerializeField]
        private List<ShopSlot> itemSlots;

        private Inventory inventory => Inventory.Instance;

        public EShopType ShopType { get => shopType; set => shopType = value; }

        private void Awake()
        {
            CategoryInitialize();

            shopList.Initialize();
            
            InitializeList();
        }

        private void Start()
        {
            inventory.OnItemChangedCallBack += delegate(bool refresh) { UpdateUI(refresh); };

            if (inventory.ItemPool.ItemCount == 0) { return; }

            UpdateUI(true);
        }

        private void Update()
        {
            if (itemShopUI.gameObject.activeSelf && KeyManager.GetKeyDown("Jump")) 
            {
                ShopWindowState(false, EShopType.None); 
            }
        }

        public void ShopWindowState(bool state, EShopType shopType) 
        {
            this.shopType = shopType;
            itemShopUI.gameObject.SetActive(state);

            GameManager.Instance.shopMode = state;

            if (shopType != EShopType.None) { SetContent(); }
        }

        private void SetContent() 
        {
            if (this.shopType == EShopType.Perchase) 
            {
                scrollRect.content = perchaseParent as RectTransform;
                perchaseParent.gameObject.SetActive(true);
                sellParent.gameObject.SetActive(false);
                shopTitle.text = "Perchase";
            }

            if (this.shopType == EShopType.Sell)
            {
                scrollRect.content = sellParent as RectTransform;
                sellParent.gameObject.SetActive(true);
                perchaseParent.gameObject.SetActive(false);
                shopTitle.text = "Sell";
            }
        }

        #region ICategoryHandler

        public void CategoryInitialize() 
        {
            foreach (CategorySelection selection in selections) { selection.categoryHandler = this; }
        }

        public void SelectCategory(ECategory category)
        {
            foreach (CategorySelection selection in selections)
            {
                if (selection.Category != category) { selection.DeSelect(); }
            }

            foreach (DetailSlot slot in shopSlots)
            {
                slot.gameObject.SetActive(slot.Item.Category == category || category == ECategory.Everything);
            }
        }

        #endregion

        private void InitializeList() 
        {
            shopList.Items.ForEach(item => 
            {
                var addItem = CreateSlot(perchaseParent);

                addItem.SetShopType(EShopType.Perchase);

                addItem.AddItem(inventory.ItemPool[item.ItemName]);

                shopSlots.Add(addItem);
            });

            inventory.ItemPool.Holds.ToList().ForEach(item =>
            {
                var addItem = CreateSlot(sellParent);

                addItem.SetShopType(EShopType.Sell);

                addItem.AddItem(item);

                itemSlots.Add(addItem);
            });
        }

        private void Trade(Item item) 
        {
            if (shopType == EShopType.Perchase && item is IItemPerchaseHandler perchase) { perchase.Perchase(1); }

            if (shopType == EShopType.Sell && item is IItemSellHandler sell) { sell.SoldOut(1); }
        }

        public void Display(DetailSlot slot)
        {
            itemDetail.text = slot.Item.Detail;

            itemSell.text = slot.Item is IItemSellHandler sell ? $"{sell.SellPrice}" : "無法出售";
            itemPerchase.text = slot.Item is IItemPerchaseHandler perchase ? $"{perchase.PerchasePrice}" : "無法購買";
        }

        #region Check Item List

        private void UpdateUI(bool refresh)
        {
            if (refresh) { RefreshList(); }

            UpdateList();
#if UNITY_STANDALONE_WIN
            if (refresh) { SetNavigation(); }
#endif
        }

        private void UpdateList()
        {
            itemCash.text = $"{inventory.ItemPool.Cash}";

            shopSlots.ForEach(shopSlot => shopSlot.UpdateCount());

            itemSlots.ForEach(shopSlot => shopSlot.UpdateCount());
        }

        private void RefreshList()
        {
            var itemCount = inventory.ItemPool.ItemCount;

            if (sellParent.childCount < itemCount)
            {
                var need = itemCount - sellParent.childCount;

                for (int i = 0; i < need; i++) { itemSlots.Add(CreateSlot(sellParent)); }
            }

            for (int i = 0; i < itemCount; i++)
            {
                var slot = itemSlots[i];
                var item = inventory.ItemPool.Holds[i];

                if (!slot.Stack.Equals(item)) { slot.AddItem(item); }
            }

            if (sellParent.childCount > itemCount) { DestroyRemain(); }

            UpdateList();
        }

        private ShopSlot CreateSlot(Transform parent)
        {
            var slotObject = Instantiate(slotPrefab, parent);
            var shopSlot = slotObject.GetComponent<ShopSlot>();

            shopSlot.Button.onClick.AddListener(delegate() { Trade(shopSlot.Item); });
            shopSlot.OnSelectCallBack = delegate() { Display(shopSlot); };
            shopSlot.OnExitCallBack = delegate() 
            { 
                itemDetail.text = string.Empty;
                itemSell.text = string.Empty;
                itemPerchase.text = string.Empty;
            };

            return shopSlot;
        }

        private void DestroyRemain()
        {
            var itemCount = inventory.ItemPool.ItemCount;

            for (int i = itemCount; i < sellParent.childCount; i++)
            {
                itemSlots.RemoveAt(i);
                Destroy(sellParent.GetChild(i).gameObject);
            }
        }

        #endregion
#if UNITY_STANDALONE_WIN
        #region Navigation Reset

        private void SetNavigation()
        {
            var length = shopSlots.Count - 1;

            for (int i = 0; i <= length; i++)
            {
                var detailSlot = shopSlots[i];
                var navigation = new Navigation();

                navigation.mode = Navigation.Mode.Explicit;

                if (i == 0)
                {
                    navigation.selectOnUp = shopSlots[length].Button;
                    navigation.selectOnDown = length != 0 ? shopSlots[i + 1].Button : shopSlots[0].Button;
                }

                if (i >= 1 && i <= length - 1)
                {
                    navigation.selectOnUp = shopSlots[i - 1].Button;
                    navigation.selectOnDown = shopSlots[i + 1].Button;
                }

                if (i == length && i != 0)
                {
                    navigation.selectOnUp = shopSlots[i - 1].Button;
                    navigation.selectOnDown = shopSlots[0].Button;
                }

                detailSlot.Button.navigation = navigation;
            }
        }

        #endregion
#endif
    }
}