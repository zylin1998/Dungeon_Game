using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        #region Item Stack

        [System.Serializable]
        public class ItemStack
        {
            [SerializeField]
            private Item item;
            [SerializeField]
            private int count;

            public Item Item => item;
            public int Count => count;

            protected ItemStack() { }

            public ItemStack(Item item)
            {
                this.item = item;
                this.count = 1;
            }

            public ItemStack(Item item, int count)
            {
                this.item = item;
                this.count = count;
            }

            public void Increase()
            {
                this.count++;
            }

            public void Increase(int count)
            {
                this.count += count;
            }

            public void Decrease()
            {
                this.count--;
            }

            public void Decrease(int count)
            {
                this.count -= count;
            }
        }

        #endregion

        #region Singleton

        public static Inventory Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) { return; }

            Instance = this;
        }

        #endregion

        [SerializeField]
        private int gold;
        [SerializeField]
        private List<ItemStack> itemList = new List<ItemStack>();

        public int Gold => gold;
        public List<ItemStack> ItemList => itemList;

        public Action<bool> OnItemChangedCallBack { get; set; }

        public void IncreaseGold(int reward)
        {
            this.gold += reward;
        }

        public void DecreaseGold(int paid)
        {
            this.gold -= paid;
        }

        #region Add OverLoad

        public bool Add(Item item)
        {
            return Add(item, 1);
        }

        public bool Add(Item item, int count)
        {
            if (item.IsDefaultItem) { return false; }

            var exist = this.itemList.Find(match => match.Item.Equals(item));

            var refresh = exist == null;

            if (!refresh) { exist.Increase(count); }

            if (refresh)
            {
                this.itemList.Add(new ItemStack(item, count));
                this.itemList.Sort((ItemStack s1, ItemStack s2) => Sort(s1.Item, s2.Item));
            }

            if (this.OnItemChangedCallBack != null) { this.OnItemChangedCallBack.Invoke(refresh); }

            return true;
        }

        #endregion

        #region Remove OverLoad

        public bool Remove(Item item)
        {
            return Remove(item, 1);
        }

        public bool Remove(Item item, int count)
        {
            var target = this.itemList.Find(match => match.Item.Equals(item));

            if (target == null) { return false; }

            var refresh = false;

            target.Decrease(count);

            if (target.Count <= 0) { refresh = itemList.Remove(target); }

            if (OnItemChangedCallBack != null) { OnItemChangedCallBack.Invoke(refresh); }

            return true;
        }

        #endregion

        #region List Sorting Func

        private int Sort(Item i1, Item i2)
        {
            return i1.Category != i2.Category ? (int)i1.Category - (int)i2.Category : i1.ItemName.CompareTo(i2.ItemName);
        }

        #endregion
    }
}