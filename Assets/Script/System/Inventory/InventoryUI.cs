using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CustomInput;

namespace InventorySystem
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        private Transform inventoryUI;
        [SerializeField]
        private Transform ItemParent;
        [SerializeField]
        private Text itemDetail;
        [SerializeField]
        private GameObject slotPrefab;
        [SerializeField]
        private CategorySelection[] selections;
        [SerializeField] 
        private List<DetailSlot> detailSlots;

        private Inventory inventory;

        private void Awake()
        {
            foreach (CategorySelection selection in selections) { selection.InventoryUI = this; }
        }

        private void Start()
        {
            inventory = Inventory.Instance;

            inventory.OnItemChangedCallBack = UpdateUI;

            if (inventory.ItemList.Count == 0) { return; }

            UpdateUI(true);
        }

        private void Update()
        {
#if UNITY_STANDALONE_WIN
            if(KeyManager.GetKeyDown("Inventory")) 
            { 
                inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf); 

                if (inventoryUI.gameObject.activeSelf) { selections[0].Select(); }
            }
#endif
            GameManager.Instance.inventoryMode = inventoryUI.gameObject.activeSelf;
        }

        private void UpdateUI(bool refresh)
        {
            CheckList(refresh);
#if UNITY_STANDALONE_WIN
            if (refresh) { SetNavigation(); }
#endif
        }

        public void SelectCategory(ECategory category) 
        {
            foreach (CategorySelection selection in selections) 
            {
                if (selection.Category != category) { selection.DeSelect(); }
            }

            foreach (DetailSlot slot in detailSlots) 
            {
                slot.gameObject.SetActive(slot.Item.Category == category || category == ECategory.Everything);
            }
        }

        public void Display(DetailSlot slot)
        {
            itemDetail.text = slot.Item.Detail;
        }

        public void ItemUsed(DetailSlot slot) 
        {
            if (slot.Item.Category == ECategory.Jewelry) { return; }

            slot.Item.Used();
        }

        #region Check Item List

        private void CheckList(bool refresh) 
        {
            Action action = refresh ? RefreshList : UpdateList;

            action.Invoke();
        }

        private void UpdateList() 
        {
            inventory.ItemList.ForEach(stack => detailSlots.Find(match => match.Item.Equals(stack.Item)).UpdateCount());
        }

        private void RefreshList() 
        {
            detailSlots.Clear();

            var itemCount = inventory.ItemList.Count;

            if (ItemParent.childCount < itemCount) { CreateSlot(); }

            for (int i = 0; i < itemCount; i++) 
            {
                var child = ItemParent.GetChild(i);
                var slot = child.GetComponent<DetailSlot>();
                var item = inventory.ItemList[i];

                if (!slot.Stack.Equals(item)) { slot.AddItem(item); }

                slot.UpdateCount();
                detailSlots.Add(slot);
            }

            if (ItemParent.childCount > itemCount) { DestroyRemain(); }
        }

        private void CreateSlot() 
        {
            var slotObject = Instantiate(slotPrefab, ItemParent);
            var detailSlot = slotObject.GetComponent<DetailSlot>();

            detailSlot.Button.onClick.AddListener(() => { ItemUsed(detailSlot); });
            detailSlot.OnSelectCallBack = () => { Display(detailSlot); };
            detailSlot.OnExitCallBack = () => { itemDetail.text = string.Empty; };
        }

        private void DestroyRemain() 
        {
            var itemCount = inventory.ItemList.Count;

            for (int i = itemCount; i < ItemParent.childCount; i++)
            {
                Destroy(ItemParent.GetChild(i).gameObject);
            }
        }

        #endregion
#if UNITY_STANDALONE_WIN
        #region Navigation Reset

        private void SetNavigation() 
        {
            var length = detailSlots.Count - 1;

            for (int i = 0; i <= length; i++) 
            {
                var detailSlot = detailSlots[i];
                var navigation = new Navigation();

                navigation.mode = Navigation.Mode.Explicit;

                if (i == 0) 
                { 
                    navigation.selectOnUp = detailSlots[length].Button;
                    navigation.selectOnDown = length != 0 ? detailSlots[i + 1].Button : detailSlots[0].Button;
                }

                if (i >= 1 && i <= length - 1)
                {
                    navigation.selectOnUp = detailSlots[i - 1].Button;
                    navigation.selectOnDown = detailSlots[i + 1].Button;
                }

                if (i == length && i != 0)
                {
                    navigation.selectOnUp = detailSlots[i - 1].Button;
                    navigation.selectOnDown = detailSlots[0].Button;
                }

                detailSlot.Button.navigation = navigation;
            }
        }

        #endregion
#endif
    }
}