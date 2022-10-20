using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public class WallSensor : MonoBehaviour
    {
        private IWallCheck controller;

        private Collider2D player;

        private void Start()
        {
            var parent = this.transform.parent;

            player = parent.GetComponent<Collider2D>();
            controller = parent.GetComponent<IWallCheck>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.isTrigger) { return; }

            var contactPoint = new List<ContactPoint2D>();

            player.GetContacts(contactPoint);

            var contactWall = contactPoint.Where(point => point.normal.x != 0).ToArray();

            var allSide = 0f;

            foreach (ContactPoint2D point in contactWall) 
            {
                //Debug.DrawRay(contactPoint[0].point, contactPoint[0].normal, Color.white);

                var side = point.normal.x;

                if (side != 0) { side = side > 0 ? 1f : -1f; }

                allSide += side; 
            }
            
            if (controller != null) 
            {
                controller.IsCollision = allSide;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (controller != null) { controller.IsCollision = 0f; }
        }
    }
}
