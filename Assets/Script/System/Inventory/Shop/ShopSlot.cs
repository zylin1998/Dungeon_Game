using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public class ShopSlot : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler, ISlotHandler<ItemPool.ItemStack>
    {
        [SerializeField]
        private Image icon;
        [SerializeField]
        protected EShopType shopType;
        [SerializeField]
        protected Text itemName;
        [SerializeField]
        protected Text itemCount;
        [SerializeField]
        protected Text itemLimit;

        public Button Button => this.GetComponent<Button>();

        public ItemPool.ItemStack Item { get; set; }

        public bool Interact { get; set; }

        public Action OnSelectCallBack { get; set; }
        public Action OnExitCallBack { get; set; }


        public void SetSlot(ItemPool.ItemStack item)
        {
            Interact = true;
            Item = item;

            icon.sprite = item.Item.Icon;
            icon.preserveAspect = true;
            icon.enabled = true;

            itemName.text = item.Item.ItemName;
            itemCount.text = $"{item.Count}";

            SetPrice(this.Item.Item);
        }

        public void ClearSlot()
        {
            Item = null;

            itemName.text = string.Empty;
            itemCount.text = string.Empty;

            icon.sprite = null;
            icon.enabled = false;

            itemLimit.text = string.Empty;
        }

        public void UpdateSlot() 
        {
            itemCount.text = $"{Item.Count}";

            SetPrice(this.Item.Item);
        }

        public void CheckSlot() 
        {
            Interact = Item != null;
        }

        public void SetShopType(EShopType shopType) 
        {
            this.shopType = shopType;
        }

        private void SetPrice(Item item) 
        {
            if (shopType == EShopType.Perchase && item is IItemPerchaseHandler perchase) 
            { 
                itemLimit.text = $"{perchase.PerchasePrice}";
            }

            if (shopType == EShopType.Sell && item is IItemSellHandler sell) 
            { 
                itemLimit.text = $"{sell.SellPrice}";
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.OnSelect(eventData);
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (OnSelectCallBack != null) { OnSelectCallBack.Invoke(); }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnExitCallBack != null) { OnExitCallBack.Invoke(); }
        }
    }
}