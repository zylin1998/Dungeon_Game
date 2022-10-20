using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        #region Singleton

        public static Inventory Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) { return; }

            Instance = this;
        }

        #endregion
        
        [SerializeField]
        private ItemPool itemPool;

        public ItemPool ItemPool => itemPool;

        
        public Action<bool> OnItemChangedCallBack { get; set; }

        public void IncreaseGold(int reward)
        {
            this.itemPool.IncreaseCash(reward);

            if (this.OnItemChangedCallBack != null) { this.OnItemChangedCallBack.Invoke(false); }
        }

        public bool DecreaseGold(int paid)
        {
            bool success = this.itemPool.DecreaseCash(paid);

            if (success && this.OnItemChangedCallBack != null) { this.OnItemChangedCallBack.Invoke(false); }

            return success;
        }

        #region Add OverLoad

        public bool Add(Item item)
        {
            return Add(item, 1);
        }

        public bool Add(Item item, int count)
        {
            if (item.IsDefaultItem) { return false; }

            var stack = this.itemPool.GetItem(item);

            if (stack == null) { return false; } 

            var refresh = stack.Count == 0;

            var success = stack.Increase(count);

            if (success && this.OnItemChangedCallBack != null) { this.OnItemChangedCallBack.Invoke(refresh); }

            return success;
        }

        #endregion

        #region Remove OverLoad

        public bool Remove(Item item)
        {
            return Remove(item, 1);
        }

        public bool Remove(Item item, int count)
        {
            var target = this.itemPool.GetItem(item);

            if (target == null) { return false; }

            var refresh = false;

            var success = target.Decrease(count);

            if (target.Count <= 0) { refresh = true; }

            if (success && OnItemChangedCallBack != null) { OnItemChangedCallBack.Invoke(refresh); }

            return success;
        }

        #endregion

        #region Trade Function

        public bool Perchase(IItemPerchaseHandler item, int count) 
        {
            var paid = DecreaseGold(item.PerchasePrice * count);

            if (paid) { this.Add(item as Item, count); }

            return paid;
        }

        public bool SoldOut(IItemSellHandler item, int count)
        {
            var success = Remove(item as Item);

            if (success) { IncreaseGold(item.SellPrice * count); }

            return success;
        }

        public bool IsEnough(int cost) { return cost <= itemPool.Cash; }

        #endregion
    }
}