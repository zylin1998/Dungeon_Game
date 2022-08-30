using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public class AttackSensor : MonoBehaviour
    {
        [SerializeField]
        private string targetTag;
        [SerializeField]
        private bool startActive;

        public Collider2D Sensor => this.GetComponent<Collider2D>();

        private float damage => this.transform.parent.GetComponent<Health>().CurrentDamage;

        private void Start()
        {
            Sensor.enabled = startActive;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(targetTag) && !collision.GetComponent<Health>().IsDead)
            {
                collision.GetComponent<IHurtAction>().Hurt(damage);
            }
        }
    }
}