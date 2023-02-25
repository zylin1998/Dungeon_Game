using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public interface IInteractable
    {
        public bool Instance { get; }

        public void Interact();
    }

    public class InteractSensor : MonoBehaviour
    {
        public Vector2 Direct { get; private set; }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Interact")) { return; }

            var interactable = collision.GetComponent<IInteractable>();

            if (interactable.Instance) { interactable.Interact(); }
        }

        public void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Interact")) { return; }

            var interactable = collision.GetComponent<IInteractable>();

            if (this.Direct.y != 0) { interactable?.Interact(); }

            (interactable as IFlipHandler)?.LookAt(this.transform.parent);
        }

        public void SetDirect(Vector2 direct)
        {
            this.Direct = direct;
        }
    }
}