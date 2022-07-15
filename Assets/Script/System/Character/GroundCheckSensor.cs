using UnityEngine;

namespace RoleSystem
{
    public class GroundCheckSensor : MonoBehaviour
    {
        private PlayerController controller;

        private void Start()
        {
            var parent = this.transform.parent;

            controller = parent.GetComponent<PlayerController>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Ground") && !collision.CompareTag("Block")) { return; }

            if (controller.VerticalVelocity <= 0)
            {
                controller.IsGround = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (controller != null) { controller.IsGround = false; }
        }
    }
}