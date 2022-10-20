using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    public class QuestBoardSlots : MonoBehaviour, ICategoryHandler, ISlotFieldHandler<QuestSlot>, IPageStateHandler
    {
        #region ISlotFieldHandler

        [SerializeField]
        private Transform content;
        [SerializeField]
        private Button cancel;
        [SerializeField]
        ISlotFieldHandler<QuestSlot>.SlotDetail detail;

        public Transform Content => content;
        public Button Cancel => cancel;

        public ISlotFieldHandler<QuestSlot>.SlotDetail Detail => detail;

        public List<QuestSlot> Slots { get; set; }
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

            CustomContainer.AddContent(this, "QuestBoard");
        }

        private void Start()
        {
            GameManager.Instance.AddPage(this);
        }

        #region ICategoryHandler

        [SerializeField]
        private List<CategorySelection> selections;

        public List<CategorySelection> Selections => selections;

        public ICategoryHandler.Category category => QuestManager.Instance.Category;

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

                var interact = slot.Interact && (this.category.Compare(slot.Item.Detail.questState, category) || this.category.Compare(category, "Everything"));

                slot.gameObject.SetActive(interact);
            });
        }

        #endregion

        #region IPageOpenHandler

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
            CustomContainer.RemoveContent(this, "QuestBoard");
        }
    }
}