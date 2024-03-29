using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public class DetailSlot : MonoBehaviour, ISlotHandler<ItemPool.ItemStack>, INaviSelectable, IPointerEnterHandler, ISelectHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image icon;
        [SerializeField]
        protected Text itemName;
        [SerializeField]
        protected Text itemCount;

        #region ISlotHandler

        public Button Button => this.GetComponent<Button>();
        
        public ItemPool.ItemStack Content { get; set; }

        public bool Interact => this.Content != null;

        public Action OnSelectCallBack { get; set; }
        public Action OnExitCallBack { get; set; }

        public void SetSlot(ItemPool.ItemStack item)
        {
            Content = item;

            icon.sprite = item.Item.Icon;
            icon.preserveAspect = true;
            icon.enabled = true;

            itemName.text = item.Item.ItemName;
            itemCount.text = $"{item.Count}";
        }

        public void ClearSlot()
        {
            Content = null;

            itemName.text = string.Empty;
            itemCount.text = string.Empty;

            icon.sprite = null;
            icon.enabled = false;
        }

        public void UpdateSlot() 
        {
            itemCount.text = $"{Content.Count}";
        }

        public void CheckSlot() 
        {
            
        }

        public void UIState(bool state) 
        {
            this.gameObject.SetActive(state);
        }

        #endregion

        #region INaviSelectable

        public Selectable Selectable => this.Button;

        public bool IsSelected { get; private set; }

        public INavigationCtrl Belonging { get; set; }

        public Vector2 ID { get; set; }

        public void Select()
        {
            this.IsSelected = true;

            if (Belonging != null) { Belonging.SetFinal(this); }
        }
        
        public void DeSelect()
        {
            this.IsSelected = false;
        }

        #endregion

        #region IPointerEvents

        public void OnPointerEnter(PointerEventData eventData) 
        {
            this.OnSelect(eventData);
        }

        public void OnSelect(BaseEventData eventData) 
        {
            if (OnSelectCallBack != null) { OnSelectCallBack.Invoke(); }

            this.Select();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnExitCallBack != null) { OnExitCallBack.Invoke(); }

            this.DeSelect();
        }

        #endregion
    }
}