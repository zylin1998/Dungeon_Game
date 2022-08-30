using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "Item Pool", menuName = "Inventory/Item Pool", order = 1)]
    public class ItemPool : ScriptableObject
    {
        #region Item Stack Class
        
        [System.Serializable]
        public class ItemStack
        {
            [SerializeField]
            private GameObject perfab;
            [SerializeField]
            private Item item;
            [SerializeField]
            private int limit;
            [SerializeField]
            private int count;

            public Item Item => item;
            public int Limit => limit;
            public int Count => count;
            public GameObject Perfab => perfab;
            public string ItemName => this.item.ItemName;

            public bool Increase()
            {
                return this.Increase(1);
            }

            public bool Increase(int count) 
            { 
                if ((this.count + count) > this.limit) { return false; }

                this.count += count;

                return true;
            }

            public bool Decrease()
            {
                return Decrease(1);
            }

            public bool Decrease(int count) 
            {
                if (count > this.count) { return false; }

                this.count -= count;

                return true;
            }

            public void SetCount(int count) 
            {
                this.count = count;
            }
        }

        #endregion

        #region Item Data

        [System.Serializable]
        public class ItemData 
        {
            public string itemName;
            public int count;

            protected ItemData() { }

            public ItemData(string name, int count) 
            {
                this.itemName = name;
                this.count = count;
            }
        }

        #endregion

        #region Packed

        [System.Serializable]
        public class Packed 
        {
            public ItemData[] items;

            public int cash;

            protected Packed() { }

            public Packed(ItemPool asset) 
            {
                this.cash = asset.cash;

                this.items = asset.items.ToList().ConvertAll(stack => new ItemData(stack.ItemName, stack.Count)).ToArray();
            }
        }

        #endregion

        [SerializeField]
        private int cash;
        [SerializeField]
        private ItemStack[] items;

        public Packed pack => new Packed(this);

        #region Cash

        public int Cash => cash;

        public void IncreaseCash(int cash) => this.cash += cash;

        public bool DecreaseCash(int cash)
        {
            if (this.cash - cash < 0) { return false; }

            this.cash -= cash;

            return true;
        }

        #endregion

        #region Item

        public ItemStack[] Items => items;

        public ItemStack[] Holds => items.Where(item => item.Count != 0).ToArray();

        public ItemStack this[string itemName] => items.ToList().Find(match => match.ItemName == itemName);

        public ItemStack this[int index] => items[index];

        public bool IsEmpty => items.All(item => item.Count == 0);

        public int ItemCount => items.Count(item => item.Count != 0);

        #endregion

        public void Initialize() 
        {
            this.cash = 0;

            this.items.ToList().ForEach(item => item.SetCount(0));
        }

        public void Initialize(Packed packed) 
        {
            this.cash = packed.cash;

            packed.items.ToList().ForEach(item => this[item.itemName].SetCount(item.count));
        }
    }
}