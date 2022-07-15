using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Diamond", menuName = "Inventory/Item/JewelryItem/Diamond", order = 1)]
    public class Diamond : JewelryItem
    {
        public override bool PickUp()
        {
            return base.PickUp();
        }

        public override void Used()
        {
            Debug.Log($"{itemName} not for using.");
        }

        public override void SoldOut(int count)
        {
            Inventory.Instance.Remove(this, count);
            Inventory.Instance.IncreaseGold(sellPrice * count);
        }
    }
}
