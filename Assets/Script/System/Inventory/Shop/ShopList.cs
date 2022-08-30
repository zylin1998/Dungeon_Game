using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem 
{
    [CreateAssetMenu(fileName = "Shop List", menuName = "Inventory/Shop List", order = 1)]
    public class ShopList : ScriptableObject
    {
        [System.Serializable]
        public class Packed
        {
            public string[] items;

            protected Packed() { }

            public Packed(ShopList asset)
            {
                var list = new List<string>();

                asset.Items.ForEach(item => list.Add(item.ItemName));
            }
        }

        [SerializeField]
        private ItemPool itemPool;
        [SerializeField]
        private ShopList defaultList;
        [SerializeField]
        private List<Item> items;

        public List<Item> Items => items;

        public void Initialize() 
        {
            this.items = defaultList.items;
        }

        public void Initialize(Packed pack) 
        {
            var list = new List<Item>();

            pack.items.ToList().ForEach(item => list.Add(itemPool[item].Item));

            items = list;
        }

        public void AddItem(Item item) 
        {
            items.Add(item);
        }

        public bool RemoveItem(Item item) 
        {
            return items.Remove(item);
        }
    }
}