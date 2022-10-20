using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class ShopSlotField : MonoBehaviour, ICategoryHandler, ISlotFieldHandler<ShopSlot>, IPageStateHandler
    {
        #region ISlotFieldHandler

        [SerializeField]
        private Transform content;
        [SerializeField]
        private ISlotFieldHandler<ShopSlot>.SlotDetail detail;
        [SerializeField]
        private Button cancel;
        [SerializeField]
        private List<CategorySelection> selections;

        public Transform Content => content;
        public Button Cancel => cancel;

        public ISlotFieldHandler<ShopSlot>.SlotDetail Detail => detail;
        public List<ShopSlot> Slots { get; set; }
        public List<CategorySelection> Selections => selections;
        public Action UICloseCallBack { get; set; }

        public void UIState(bool state)
        {
            if (!state && UICloseCallBack != null) { UICloseCallBack.Invoke(); }

            this.PageState = state;

            this.gameObject.SetActive(state);
        }

        #endregion

        protected void Awake() 
        {
            CategoryInitialize();

            CustomContainer.AddContent(this, "Shop");
        }

        private void Start()
        {
            GameManager.Instance.AddPage(this);

            UIState(false);
        }

        #region ICategoryHandler

        public ICategoryHandler.Category category => Inventory.Instance.ItemPool.Category;

        public void SelectFirst()
        {
            if (selections.Any()) { selections.FirstOrDefault().Select(); }

            if (Slots.Any()) { Slots.FirstOrDefault().GetComponent<Selectable>().Select(); }
        }

        public void CategoryInitialize()
        {
            selections.ForEach(selection => selection.categoryHandler = this);
        }

        public void SelectCategory(string category)
        {
            selections.Where(deselect => deselect.Category != category).ToList().ForEach(selection => selection.DeSelect());

            Slots.ForEach(slot =>  
            {
                slot.CheckSlot();

                var interact = slot.Interact && (this.category.Compare(slot.Item.Item.Category, category) || this.category.Compare(category, "Everything"));

                slot.gameObject.SetActive(interact);
            });
        }

        #endregion

        #region

        public bool PageState { get; private set; }

        #endregion

        [SerializeField]
        protected Text cash;

        public void SetCash(int cash) 
        {
            this.cash.text = cash.ToString();
        }

        private void OnDestroy()
        {
            CustomContainer.RemoveContent(this, "Shop");
        }
    }
}