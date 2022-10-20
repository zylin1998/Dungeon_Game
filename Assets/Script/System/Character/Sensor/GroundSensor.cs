using UnityEngine;

namespace RoleSystem
{
    public class GroundSensor : MonoBehaviour
    {
        private IGroundCheck controller;

        private void Start()
        {
            var parent = this.transform.parent;

            controller = parent.GetComponent<IGroundCheck>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Ground") && !collision.CompareTag("Block")) { return; }

            if (controller.VerticalVelocity <= 0)
            {
                controller.IsGround = true;

                controller.Land();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (controller != null) { controller.IsGround = false; }
        }
    }
}