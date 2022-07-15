using UnityEngine;

namespace InventorySystem
{
    #region Catogory Enum

    [System.Serializable]
    public enum ECategory
    {
        Everything,
        Consume,
        Jewelry,
        Gold
    }

    #endregion

    public abstract class Item : ScriptableObject
    {
        [Header("道具資訊")]
        [SerializeField]
        protected string itemName = "Item";
        [SerializeField]
        protected Sprite icon = null;
        [SerializeField]
        protected ECategory category = ECategory.Everything;
        [SerializeField]
        protected string detail = "Detail";

        [SerializeField]
        protected bool isDefaultItem = false;

        public string ItemName => itemName;
        public string Detail => detail;
        public Sprite Icon => icon;
        public ECategory Category => category;

        public bool IsDefaultItem => isDefaultItem;

        public abstract bool PickUp();

        public abstract void Used();
    }
}