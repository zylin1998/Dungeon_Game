using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "White Bottle", menuName = "Inventory/Item/ConsumeItem/White Bottle", order = 1)]
    public class WhiteBottle : ConsumeItem
    {
        [SerializeField]
        protected int healMP;

        public int HealMP => healMP;

        public override bool PickUp()
        {
            return base.PickUp();
        }

        public override void Used()
        {
            base.Used();

            Debug.Log($"{itemName} used.");

            //Heal Player
        }

        public override void Perchase(int count)
        {
            Inventory.Instance.Add(this, count);
            Inventory.Instance.DecreaseGold(perchasePrice * count);
        }

        public override void SoldOut(int count)
        {
            Inventory.Instance.Remove(this, count);
            Inventory.Instance.IncreaseGold(sellPrice * count);
        }
    }
}