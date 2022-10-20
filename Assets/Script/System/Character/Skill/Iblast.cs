using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public class Iblast : Magic
    {
        [SerializeField]
        private float speed;

        private void Start()
        {
            Destroy(this, 7f);
        }

        private void Update()
        {
            var velocity = Vector2.right * this.transform.localScale.x * speed * Time.deltaTime;

            this.transform.Translate(velocity);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) 
            {
                collision.GetComponent<IHurtAction>().Hurt(this.damage); 
                
                Destroy(this.gameObject, 0.05f); 
            }
        }

        protected override void OnTriggerStay2D(Collider2D collision)
        {

        }
    }
}