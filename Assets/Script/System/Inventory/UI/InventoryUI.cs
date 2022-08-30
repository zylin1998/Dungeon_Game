using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CustomInput;

namespace InventorySystem
{
    public class InventoryUI : MonoBehaviour, ICategoryHandler
    {
        [SerializeField]
        private Transform inventoryUI;
        [SerializeField]
        private Transform ItemParent;
        [SerializeField]
        private Text itemDetail;
        [SerializeField]
        private Text itemSell;
        [SerializeField]
        private Text itemPerchase;
        [SerializeField]
        private Text itemCash;
        [SerializeField]
        private GameObject slotPrefab;
        [SerializeField]
        private CategorySelection[] selections;
        [SerializeField] 
        private List<DetailSlot> detailSlots;

        private Inventory inventory => Inventory.Instance;

        #region Script Behaviour

        private void Awake()
        {
            CategoryInitialize();
        }

        private void Start()
        {
            inventory.OnItemChangedCallBack += UpdateUI;

            if (inventory.ItemPool.IsEmpty) { return; }

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

        #endregion

        #region ICategoryHandler

        public void CategoryInitialize() 
        {
            foreach (CategorySelection selection in selections) { selection.categoryHandler = this; }
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

        #endregion

        #region Slot Function

        public void Display(DetailSlot slot)
        {
            itemDetail.text = slot.Item.Detail;

            itemSell.text = slot.Item is IItemSellHandler sell ? $"{sell.SellPrice}" : "無法出售";
            itemPerchase.text = slot.Item is IItemPerchaseHandler perchase ? $"{perchase.PerchasePrice}" : "無法購買";
        }

        public void ItemUsed(DetailSlot slot) 
        {
            if (slot.Item.Category == ECategory.Jewelry) { return; }

            slot.Item.Used();
        }

        #endregion

        #region Check Item List

        private void UpdateUI(bool refresh)
        {
            CheckList(refresh);
#if UNITY_STANDALONE_WIN
            if (refresh) { SetNavigation(); }
#endif
        }

        private void CheckList(bool refresh) 
        {
            Action action = refresh ? RefreshList : UpdateList;

            action.Invoke();
        }

        private void UpdateList() 
        {
            itemCash.text = $"{inventory.ItemPool.Cash}";

            detailSlots.ForEach(detailSlot => detailSlot.UpdateCount());
        }

        private void RefreshList() 
        {
            var itemCount = inventory.ItemPool.ItemCount;

            if (ItemParent.childCount < itemCount) 
            {
                var need = itemCount - ItemParent.childCount;

                for (int i = 0; i < need; i++) { detailSlots.Add(CreateSlot()); } 
            }

            for (int i = 0; i < itemCount; i++) 
            {
                var slot = detailSlots[i];
                var item = inventory.ItemPool.Holds[i];

                if (!slot.Stack.Equals(item)) { slot.AddItem(item); }
            }

            if (ItemParent.childCount > itemCount) { DestroyRemain(); }

            UpdateList();
        }

        private DetailSlot CreateSlot() 
        {
            var slotObject = Instantiate(slotPrefab, ItemParent);
            var detailSlot = slotObject.GetComponent<DetailSlot>();

            detailSlot.Button.onClick.AddListener(() => { ItemUsed(detailSlot); });
            detailSlot.OnSelectCallBack = () => { Display(detailSlot); };
            detailSlot.OnExitCallBack = () => { itemDetail.text = string.Empty; };

            return detailSlot;
        }

        private void DestroyRemain() 
        {
            var itemCount = inventory.ItemPool.ItemCount;

            for (int i = itemCount; i < ItemParent.childCount; i++)
            {
                detailSlots.RemoveAt(i);
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