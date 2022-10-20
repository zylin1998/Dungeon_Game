using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public class Magic : MonoBehaviour
    {
        [SerializeField]
        protected float damage;

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(string.Format("Entered gameobject name: {0}, tag: {1}", collision.name, collision.tag));
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            Debug.Log(string.Format("Stayed gameobject name: {0}, tag: {1}", collision.name, collision.tag));
        }

        public void SetDamage(float damage)
        {
            this.damage = damage;
        }
    }
}