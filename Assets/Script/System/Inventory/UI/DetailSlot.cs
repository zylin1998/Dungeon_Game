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
        protected Text itemName;
        [SerializeField]
        protected Text itemCount;

        public Action OnSelectCallBack { get; set; }
        public Action OnExitCallBack { get; set; }

        public override void AddItem(ItemPool.ItemStack item)
        {
            base.AddItem(item);

            itemName.text = item.Item.ItemName;
            itemCount.text = $"{item.Count}";
        }

        public virtual void UpdateCount() 
        {
            itemCount.text = $"{item.Count}";
        }

        public void OnPointerEnter(PointerEventData eventData) 
        {
            this.OnSelect(eventData);
        }

        public virtual void OnSelect(BaseEventData eventData) 
        {
            if (OnSelectCallBack != null) { OnSelectCallBack.Invoke(); }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnExitCallBack != null) { OnExitCallBack.Invoke(); }
        }
    }
}