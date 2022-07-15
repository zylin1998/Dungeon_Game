using UnityEngine;
using CustomInput;

namespace InventorySystem
{
    public class ItemPickup : EventTrigger
    {
        [Header("Item")]
        [SerializeField] private Item item;

        public Item Item => item;

        #region Trigger Event

        protected void Start()
        {
            if (invokeState == EInvokeState.Start) { StartInvoke(); }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) { return; }

            if (invokeState != EInvokeState.Passive) { return; }

            isTriggered = item.PickUp();

            if (isTriggered)
            {
                if (destroy) { Destroy(gameObject); }
            }
        }

        protected override void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) { return; }

            if (invokeState != EInvokeState.Interact) { return; }

            InteractHint(!isTriggered);

            if (KeyManager.GetAxis("Vertical") != 0)
            {
                isTriggered = item.PickUp();

                if (isTriggered)
                {
                    InteractHint(false);

                    if (destroy) { Destroy(gameObject); }
                }
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) { return; }

            isTriggered = false;

            InteractHint(false);
        }

        protected override void StartInvoke()
        {
            if (invokeState != EInvokeState.Start) { return; }
        }

        #endregion
    }
}