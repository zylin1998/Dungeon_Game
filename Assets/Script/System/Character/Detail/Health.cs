using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoleSystem
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private CharacterDetailAsset detail;

        [SerializeField]
        private PhysicalDetail physical;
        [SerializeField]
        private WeaponDetail weapon;

        public bool IsDead { get; private set; }

        public float CurrentDamage => weapon.MuiltyDamage;

        public float NormalizedLife => physical.Life / detail.DefaultPhysical.Life;
        public float NormalizedMP => physical.MP / detail.DefaultPhysical.MP;

        public int WeaponLevel => weapon.Level;
        public float WeaponMuiltilpe => weapon.Muiltiple[weapon.Level];

        public void Hurt(float injury) 
        {
            float finalInjury = injury * (1 / (1 + physical.Defend * 0.01f));

            physical.SetLife(Mathf.Clamp(physical.Life - finalInjury, 0f, detail.DefaultPhysical.Life));

            if (physical.IsDead) { IsDead = true; }
        }

        public bool UseSkill(float consume) 
        {
            if (physical.MP < consume) { return false; }

            physical.SetLife(Mathf.Clamp(physical.MP - consume, 0f, detail.DefaultPhysical.MP));

            return true;
        }

        public void UpgradeDamage() 
        {
            weapon.Upgrade();
        }

        public void SingleOnly() 
        {
            detail.SingleOnly(ref physical, ref weapon);
        }

        public void Production() 
        {
            detail.Production(ref physical, ref weapon);
        }

        public bool WeaponUpgrade() 
        {
            var canUpgrade = weapon.Level < weapon.MaxLevel && weapon.Level >= 0;

            if (canUpgrade) { weapon.Upgrade(); }

            return canUpgrade;
        }
    }
}