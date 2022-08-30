using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class ShopSlot : DetailSlot
    {
        [SerializeField]
        protected Text itemLimit;
        [SerializeField]
        protected ItemShopUI.EShopType shopType;

        public void SetShopType(ItemShopUI.EShopType shopType) 
        {
            this.shopType = shopType;
        }

        public override void AddItem(ItemPool.ItemStack item)
        {
            base.AddItem(item);

            if (shopType == ItemShopUI.EShopType.Perchase) { itemLimit.text = $"{item.Limit}"; }

            if (shopType == ItemShopUI.EShopType.Sell) { itemLimit.text = $"{item.Count}"; }
        }

        public override void UpdateCount() 
        {
            base.UpdateCount();

            if (shopType == ItemShopUI.EShopType.Sell) { itemLimit.text = $"{item.Count}"; }
        }
    }
}