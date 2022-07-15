using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public class TargetSensor : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float distance;

        public Transform Target => target;
        public float Distance => distance;

        private bool IsPlayer(Collider2D collision) => collision.CompareTag("Player");

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsPlayer(collision))
            {
                target = collision.transform;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (IsPlayer(collision)) 
            {
                var enemy = transform.parent;

                distance = Mathf.Abs(enemy.position.x - target.position.x);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (IsPlayer(collision))
            {
                target = null;
            }
        }
    }
}