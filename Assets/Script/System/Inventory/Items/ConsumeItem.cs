using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public abstract class ConsumeItem : Item, IItemSellHandler, IItemPerchaseHandler
    {
        [Header("交易價格")]
        [SerializeField]
        protected int sellPrice;
        [SerializeField]
        protected int perchasePrice;

        public int SellPrice => sellPrice;
        public int PerchasePrice => perchasePrice;

        public override bool PickUp()
        {
            return Inventory.Instance.Add(this);
        }

        public override void Used()
        {
            Inventory.Instance.Remove(this);
        }

        public abstract void Perchase(int count);

        public abstract void SoldOut(int count);
    }
}