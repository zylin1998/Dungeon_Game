using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem 
{
    [CreateAssetMenu(fileName = "Shop List", menuName = "Inventory/Shop List", order = 1)]
    public class ShopList : PackableObject
    {
        [System.Serializable]
        public class Pack : PackableObjectPack
        {
            public string[] items;

            protected Pack() { }

            public Pack(ShopList asset)
            {
                this.items = asset.Items.ConvertAll(item => item.ItemName).ToArray();
            }
        }

        [SerializeField]
        private ItemPool itemPool;
        [SerializeField]
        private List<Item> items;

        public List<Item> Items => items;

        public override IPackableHandler.BasicPack Packed => new Pack(this);

        public override void Initialized() 
        {
            this.items = Resources.Load<ShopList>(Path.Combine("Inventory", "Default List")).Items;
        }

        public override void Initialized(IPackableHandler.BasicPack basicPack) 
        {
            if (basicPack is Pack pack) 
            {
                items = pack.items.ToList().ConvertAll(item => itemPool.GetItem(item).Item);
            }
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