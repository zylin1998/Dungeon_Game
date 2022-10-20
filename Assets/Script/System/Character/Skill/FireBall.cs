using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public class FireBall : Magic
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private GameObject explosion;

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

        private void OnDestroy()
        {
            var parent = GameObject.Find("MagicParent").transform;

            var explosion = Instantiate(this.explosion, this.transform.position, this.transform.rotation, parent);
            explosion.transform.localScale = new Vector3(2f, 2f, 2f);
            explosion.transform.position = transform.position + new Vector3(0, 0.8f, 0);

            explosion.GetComponent<Magic>().SetDamage(this.damage);
        }
    }
}