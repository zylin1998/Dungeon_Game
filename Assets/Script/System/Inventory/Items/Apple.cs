using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Apple", menuName = "Inventory/Item/ConsumeItem/Apple", order = 1)]
    public class Apple : ConsumeItem
    {
        [Header("道具效果")]
        [SerializeField]
        protected int healHP;

        public int HealHP => healHP;

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
            Inventory.Instance.Perchase(this, count);
        }

        public override void SoldOut(int count)
        {
            Inventory.Instance.SoldOut(this, count);
        }
    }
}