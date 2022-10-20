using System;
using System.Collections;
using UnityEngine;

namespace RoleSystem
{
    public abstract class Enemy : MonoBehaviour, IHurtAction
    {
        [Header("�ĤH�򥻸�T")]
        [SerializeField]
        protected CharacterDetailAsset characterDetail;
        [SerializeField]
        protected string enemyName;
        public bool isFlip;

        #region is now uncontrollable

        protected bool uncontrollable
        {
            get
            {
                foreach (string action in actionDetail.UnControllableAction)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName($"{enemyName}_{action}")) { return true; }
                }

                if (isDead) { return true; }

                return false;
            }
        }

        #endregion

        protected Animator animator;
        protected ActionDetail actionDetail;
        protected Health health;

        protected Vector3 scale;

        protected bool isDead { get; set; }

        #region �ʧ@��H

        protected abstract void Move();

        protected abstract void Attack();

        protected abstract void Dead();

        public abstract void Hurt(float injury);

        #endregion

        #region �ʧ@�ƥ�Ĳ�o

        protected Action onPlayingCallBack;

        protected virtual IEnumerator AnimatorPlaying(string name)
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(name)) { yield return null; }

            while (animator.GetCurrentAnimatorStateInfo(0).IsName(name))
            {
                onPlayingCallBack.Invoke();

                yield return null;
            }
        }

        #endregion

        public void Flip()
        {
            var newScale = scale;

            newScale.x *= isFlip ? 1f : -1f;

            transform.localScale = newScale;
        }
    }
}