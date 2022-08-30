using UnityEngine;

namespace RoleSystem
{
    #region Action Detail Class

    [System.Serializable]
    public class ActionDetail 
    {
        [Range(1f, 10f), SerializeField]
        private float walkSpeed;
        [Range(5f, 15f), SerializeField]
        private float dashSpeed;
        [Range(10f, 40f), SerializeField]
        private float jumpForce;
        [Range(1.2f, 3.5f), SerializeField]
        private float dashCoolDown;
        [Range(1f, 2f), SerializeField]
        private int maxJumpCount;
        [Range(0.2f, 1f), SerializeField]
        private float maxJumpHold;
        [SerializeField]
        private string[] unControllableAction;

        public float WalkSpeed => walkSpeed;
        public float DashSpeed => dashSpeed;
        public float JumpForce => jumpForce;
        public float DashCoolDown => dashCoolDown;
        public int MaxJumpCount => maxJumpCount;
        public float MaxJumpHold => maxJumpHold;
        public string[] UnControllableAction => unControllableAction;
    }

    #endregion

    #region Physical Detail Class

    [System.Serializable]
    public class PhysicalDetail 
    {
        [SerializeField]
        private float life;
        [SerializeField]
        private float mp;
        [SerializeField]
        private float defend;

        public float Life => life;
        public float MP => mp;
        public float Defend => defend;

        public bool IsDead => life == 0;

        public PhysicalDetail() { }

        public PhysicalDetail(PhysicalDetail detail) 
        {
            this.life = detail.Life;
            this.mp = detail.MP;
            this.defend = detail.Defend;
        }

        public void SetLife(float value) => this.life = value; 

        public void SetMP(float value) => this.mp = value;

        public void SetDefend(float value) => this.defend = value;
    }

    #endregion

    #region Weapon Detail Class
    
    [System.Serializable]
    public class WeaponDetail
    {
        [SerializeField]
        private int level;
        [SerializeField]
        private int maxLevel;
        [SerializeField]
        private float damage;
        [SerializeField]
        private float[] muiltiple;

        public int Level => this.level;
        public int MaxLevel => this.maxLevel;
        public bool IsMax => this.level == this.maxLevel;
        public float Damage => this.damage;
        public float[] Muiltiple => this.muiltiple;
        public float Rate => this.muiltiple[this.level];
        public float MuiltyDamage => this.damage * this.muiltiple[level];

        protected WeaponDetail() { }

        public WeaponDetail(WeaponDetail weapon)
        {
            this.level = weapon.level;
            this.maxLevel = weapon.maxLevel;
            this.damage = weapon.damage;
            this.muiltiple = weapon.muiltiple;
        }

        public void Upgrade() => this.level++;
    }

    #endregion

    [CreateAssetMenu(fileName = "Character Detail Asset", menuName = "Character/Character Detail Asset", order = 1)]
    public class CharacterDetailAsset : ScriptableObject
    {
        #region Packed Class

        [System.Serializable]
        public class Packed 
        {
            public PhysicalDetail physical;
            public WeaponDetail weapon;

            protected Packed() { }

            public Packed(CharacterDetailAsset asset) 
            {
                physical = asset.SinglePhysical;
                weapon = asset.SingleWeapon;
            }
        }

        #endregion

        [Header("Action Detail")]
        [SerializeField]
        private ActionDetail actionDetail;
        [Header("Physical Detail")]
        [SerializeField]
        private PhysicalDetail defaultPhysical;
        [Header("Weapon Detail")]
        [SerializeField]
        private WeaponDetail defaultWeapon;
        
        public PhysicalDetail SinglePhysical { get; private set; }
        public WeaponDetail SingleWeapon { get; private set; }
        
        public Packed Pack => new Packed(this);

        public ActionDetail ActionDetail => actionDetail;
        public PhysicalDetail DefaultPhysical => defaultPhysical;

        public void Initialize()
        {
            SinglePhysical = new PhysicalDetail(defaultPhysical);
            SingleWeapon = new WeaponDetail(defaultWeapon);
        }

        public void Initialize(Packed packed) 
        {
            SinglePhysical = packed.physical;
            SingleWeapon = packed.weapon;
        }

        public void SingleOnly(ref PhysicalDetail physical, ref WeaponDetail weapon) 
        {
            physical = SinglePhysical;
            weapon = SingleWeapon;
        }

        public void Production(ref PhysicalDetail physical, ref WeaponDetail weapon)
        {
            physical = new PhysicalDetail(defaultPhysical);
            weapon = new WeaponDetail(defaultWeapon);
        }
    }
}