using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class DetailSlotField : MonoBehaviour, ISlotFieldHandler<ItemPool.ItemStack>, ICategoryHandler, IPageStateHandler, INaviPanelCtrl
    {
        protected void Awake() 
        {
            CategoryInitialize();

            CustomContainer.AddContent(this, "Inventory");
        }

        private void Start()
        {
            GameManager.Instance.AddPage(this);

            this.GetNaviPanels();

            UIState(false);
        }

        #region INaviPanelCtrl

        public List<INaviPanel> NaviPanels { get; private set; }

        public INaviPanel CurrentPanel { get; private set; }

        public INaviPanel Initial => this.NaviPanels.Find(n => n.Initial);

        public void GetNaviPanels() 
        {
            this.NaviPanels = CustomContainer.GetContents<SelectableCollect>("Inventory").ConvertAll(c => c as INaviPanel);
        }

        public void SelectPanel(Vector2 direct) 
        {
            var panelDir = new Vector2(direct.x, 0);
            var selDir = new Vector2(0, direct.y);

            if (panelDir != Vector2.zero)
            {
                var panel = this.CurrentPanel.FindPanel(panelDir);

                if (panel != null) 
                {
                    this.CurrentPanel = panel;

                    var select = (this.CurrentPanel as SelectableCollect)?.FinalSelect;

                    this.INaviSelectableSelect(select);
                }
            }

            if (selDir != Vector2.zero) 
            {
                var select = (this.CurrentPanel as SelectableCollect)?.FinalSelect?.FindNavi(selDir);

                this.INaviSelectableSelect(select);
            }
        }

        public void PanelInitialize ()
        {
            if (this.Initial is SelectableCollect initial) { initial.Selectables.First().Select(); }

            var normal = this.NaviPanels.ToList().Find(n => !n.Initial);

            if (normal != null) 
            {
                this.CurrentPanel = normal;

                if (this.CurrentPanel is SelectableCollect collect)
                {
                    INaviSelectableSelect(collect.Selectables.Any() ? collect.Selectables.First() : null);
                }
            }
        }

        private void INaviSelectableSelect(INaviSelectable selectable) 
        {
            if (selectable == null) { return; }

            (selectable as UnityEngine.EventSystems.ISelectHandler).OnSelect(null);

            selectable.Selectable.Select();
        }

        #endregion

        #region ISlotFieldHandler

        [SerializeField]
        private Transform content;
        [SerializeField]
        private Button cancel;
        [SerializeField]
        ISlotFieldHandler<ItemPool.ItemStack>.SlotDetail detail;

        public Transform Content => content;
        public Button Cancel => cancel;

        public ISlotFieldHandler<ItemPool.ItemStack>.SlotDetail Detail => detail;
       
        public List<ISlotHandler<ItemPool.ItemStack>> Slots { get; set; }
        public Action UICloseCallBack { get; set; }

        public void UIState(bool state)
        {
            if (!state && UICloseCallBack != null) { UICloseCallBack.Invoke(); }

            if (state) { this.PanelInitialize(); }

            this.PageState = state;

            this.gameObject.SetActive(state);
        }

        #endregion

        #region ICategoryHandler

        [SerializeField]
        private List<CategorySelection> selections;

        public List<CategorySelection> Selections => selections;
        
        public ICategoryHandler.Category category => Inventory.Instance.ItemPool.Category;

        public void SelectFirst()
        {
            if (selections.Any()) { selections.FirstOrDefault().Select(); }

            if (Slots.Any()) { Slots.FirstOrDefault().Selectable.Select(); }
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
                var categories = new string[] { slot.Content?.Item.Category, "Everything" };

                var interact = slot.Interact && this.category.Compare(category, categories);

                slot.UIState(interact);
            });
        }

        #endregion

        #region IPageStateHandler
        
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
            CustomContainer.RemoveContent(this, "Inventory");
        }
    }
}