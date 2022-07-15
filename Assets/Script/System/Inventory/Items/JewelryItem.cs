using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public abstract class JewelryItem : Item, IItemSellHandler
    {
        [Header("交易價格")]
        [SerializeField]
        protected int sellPrice;

        public int SellPrice => sellPrice;

        public override bool PickUp()
        {
            return Inventory.Instance.Add(this);
        }

        public abstract void SoldOut(int count);
    }
}