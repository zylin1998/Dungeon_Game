using UnityEngine;

public class GroundCheckSensor : MonoBehaviour
{
    private CharacterController controller;

    private void Start()
    {
        var parent = this.transform.parent;

        controller = parent.GetComponent<CharacterController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!collision.CompareTag("Ground") && !collision.CompareTag("Block")) { return; }

        if(controller.VerticalVelocity <= 0) 
        {
            controller.IsGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (controller != null) { controller.IsGround = false; }
    }
}
