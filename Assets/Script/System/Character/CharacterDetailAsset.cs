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
        private float damage;
        [SerializeField]
        private float defend;

        public float Life => life;
        public float MP => mp;
        public float Damage => damage;
        public float Defend => defend;
    }

    #endregion

    [CreateAssetMenu(fileName = "Character Detail Asset", menuName = "Character/Character Detail Asset", order = 1)]
    public class CharacterDetailAsset : ScriptableObject
    {
        [Header("Action Detail")]
        [SerializeField]
        private ActionDetail actionDetail;
        [Header("Physical Detail")]
        [SerializeField]
        private PhysicalDetail physicalDetail;

        public ActionDetail ActionDetail => actionDetail;
        public PhysicalDetail PhysicalDetail => physicalDetail;
    }
}