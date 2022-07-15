using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private CharacterDetailAsset characterDetail;
        [SerializeField]
        private float currentLife;
        [SerializeField]
        private float currentMP;
        [SerializeField]
        private float currentDamage;
        [SerializeField]
        private float currentDefend;

        private PhysicalDetail physicalDetail;

        public bool dead { get; private set; }

        public float CurrentDamage => currentDamage;

        private void Awake()
        {
            physicalDetail = characterDetail.PhysicalDetail;

            currentLife = physicalDetail.Life;
            currentMP = physicalDetail.MP;
            currentDamage = physicalDetail.Damage;
            currentDefend = physicalDetail.Defend;
        }

        public void Hurt(float injury) 
        {
            float finalInjury = injury * (1 / (1 + currentDefend * 0.01f));

            currentLife = Mathf.Clamp(currentLife - finalInjury, 0f, physicalDetail.Life);

            if (currentLife <= 0) { dead = true; }
        }

        public bool UseSkill(float consume) 
        {
            if (currentMP < consume) { return false; }

            currentMP = Mathf.Clamp(currentMP - consume, 0f, physicalDetail.MP);

            return true;
        }
    }
}