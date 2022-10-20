using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public interface IItemSellHandler
    {
        public int SellPrice { get; }

        public void SoldOut(int count);
    }

    public interface IItemPerchaseHandler
    {
        public int PerchasePrice { get; }

        public void Perchase(int count);
    }
}