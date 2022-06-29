using UnityEngine;

public class WallCheckSensor : MonoBehaviour
{
    private CharacterController controller;
    
    ContactPoint2D[] contactPoint = new ContactPoint2D[3];

    private void Start()
    {
        var parent = this.transform.parent;

        controller = parent.GetComponent<CharacterController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.isTrigger) { return; }

        collision.GetContacts(contactPoint);

        foreach (ContactPoint2D contact in contactPoint)
        {
            //Debug.Log($"{contact.collider.name} hit {contact.otherCollider.name} {contact.normal.x}.");
            //Debug.DrawRay(contact.point, contact.normal, Color.white);
            if (contact.normal.x == 0) { continue; }

            if (controller != null) { controller.IsCollision = contact.normal.x; }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (controller != null) { controller.IsCollision = 0; }
    }
}
