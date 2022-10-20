using UnityEngine;

namespace RoleSystem
{
    public class Explosion : Magic
    {
        private Collider2D collision;

        private void Awake()
        {
            collision = this.GetComponent<Collider2D>();
        }

        private void Start()
        {
            collision.enabled = false;
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {

        }

        protected override void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) { collision.GetComponent<IHurtAction>().Hurt(this.damage); }
        }

        #region 動畫事件
        /// <summary>
        /// Animator Function, Dont Erase
        /// </summary>
        /// <param name="state"></param>
        private void TriggerState(string state)
        {
            bool isOpen = false;

            if (state == "Open") { isOpen = true; }

            if (state == "Close") { isOpen = false; }

            collision.enabled = isOpen;
        }

        private void Destroy()
        {
            GameObject.Destroy(this.gameObject);
        }

        #endregion
    }
}