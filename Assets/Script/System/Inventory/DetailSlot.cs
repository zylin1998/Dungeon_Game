using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public class DetailSlot : InventorySlot, IPointerEnterHandler, ISelectHandler, IPointerExitHandler
    {
        [Header("道具詳情文本")]
        [SerializeField]
        private Text itemName;
        [SerializeField]
        private Text itemCount;

        public Action OnSelectCallBack { get; set; }
        public Action OnExitCallBack { get; set; }

        public override void AddItem(Inventory.ItemStack item)
        {
            base.AddItem(item);

            itemName.text = item.Item.ItemName;
            itemCount.text = $"{item.Count}";
        }

        public void UpdateCount() 
        {
            itemCount.text = $"{item.Count}";
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