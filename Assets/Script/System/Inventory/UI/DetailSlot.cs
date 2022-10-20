using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public class DetailSlot : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler, ISlotHandler<ItemPool.ItemStack>
    {
        [SerializeField]
        private Image icon;
        [SerializeField]
        protected Text itemName;
        [SerializeField]
        protected Text itemCount;

        public Button Button => this.GetComponent<Button>();

        public ItemPool.ItemStack Item { get; set; }

        public bool Interact { get; set; }

        public Action OnSelectCallBack { get; set; }
        public Action OnExitCallBack { get; set; }

        public void SetSlot(ItemPool.ItemStack item)
        {
            Item = item;

            icon.sprite = item.Item.Icon;
            icon.preserveAspect = true;
            icon.enabled = true;

            itemName.text = item.Item.ItemName;
            itemCount.text = $"{item.Count}";
        }

        public void ClearSlot()
        {
            Item = null;

            itemName.text = string.Empty;
            itemCount.text = string.Empty;

            icon.sprite = null;
            icon.enabled = false;
        }

        public void UpdateSlot() 
        {
            itemCount.text = $"{Item.Count}";
        }

        public void CheckSlot() 
        {
            Interact = Item != null;
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