using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class ShopManager : MonoBehaviour
    {
        #region Singleton

        public static ShopManager Instance { get; private set; }

        private void Awake()
        {
            shopList.Initialized();

            if (Instance != null) { return; }

            Instance = this;
        }

        #endregion

        [SerializeField]
        private ShopList shopList;
        [SerializeField]
        private EShopType shopType;
        [SerializeField]
        private ItemShopUI shopUI;

        public EShopType ShopType => shopType;

        public ShopList ShopList => shopList;

        public void Trade(Item item)
        {
            if (shopType == EShopType.None) { return; }

            if (shopType == EShopType.Perchase && item is IItemPerchaseHandler perchase) { perchase.Perchase(1); }

            if (shopType == EShopType.Sell && item is IItemSellHandler sell) { sell.SoldOut(1); }
        }

        public void OpenShop(EShopType shopType) 
        {
            this.shopType = shopType;

            shopUI.UIState(true);
        }

        public void SetCategory(ECategory category) 
        {

        }
    }
}