using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class ShopDetailPanel : MonoBehaviour, IDetailPanelHandler<ItemPool.ItemStack>
    {
        [SerializeField]
        protected Text itemName;
        [SerializeField]
        protected Text description;
        [SerializeField]
        protected Text perchasePrice;
        [SerializeField]
        protected Text sellPrice;

        private void Awake()
        {
            CustomContainer.AddContent(this, "Shop");
        }

        public void SetDetail(ItemPool.ItemStack stack)
        {
            var item = stack.Item;

            itemName.text = item.ItemName;
            description.text = item.Detail;

            perchasePrice.text = item is IItemPerchaseHandler perchase ? $"{perchase.PerchasePrice}" : "無法購買";
            sellPrice.text = item is IItemSellHandler sell ? $"{sell.SellPrice}" : "無法出售";
        }

        public void Clear() 
        {
            itemName.text = string.Empty;
            description.text = string.Empty;

            perchasePrice.text = string.Empty;
            sellPrice.text = string.Empty;
        }

        private void OnDestroy()
        {
            CustomContainer.RemoveContent(this, "Shop");
        }
    }
}