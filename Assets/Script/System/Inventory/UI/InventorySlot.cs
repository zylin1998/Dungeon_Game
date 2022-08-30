using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class InventorySlot : MonoBehaviour
    {
        [Header("道具欄基本物件")]
        [SerializeField]
        protected Button button;
        [SerializeField]
        protected Image icon;
        [SerializeField]
        protected ItemPool.ItemStack item;

        public Item Item => item.Item;
        public ItemPool.ItemStack Stack => item;
        public Button Button => button;

        public virtual void AddItem(ItemPool.ItemStack item)
        {
            this.item = item;

            icon.sprite = item.Item.Icon;
            icon.preserveAspect = true;
            icon.enabled = true;
        }

        public void ClearSlot()
        {
            item = null;

            icon.sprite = null;
            icon.enabled = false;
        }

        public void CheckSlot()
        {
            if (item == null) { button.interactable = false; }

            else { button.interactable = true; }
        }
    }
}