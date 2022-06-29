using UnityEngine;

[CreateAssetMenu(fileName = "Character Detail Asset", menuName = "Character/Character Detail Asset", order = 1)]
public class CharacterDetailAsset : ScriptableObject
{
    [Header("Character Detail")]
    [Range(1f, 10f), SerializeField]
    private float walkSpeed;
    [Range(5f, 15f), SerializeField]
    private float dashSpeed;
    [Range(10f, 40f), SerializeField]
    private float jumpForce;
    [Range(1.2f, 3.5f), SerializeField]
    private float dashCoolDown;
    [SerializeField]
    private string[] uncontrollableAction;

    public float WalkSpeed => walkSpeed;
    public float DashSpeed => dashSpeed;
    public float JumpForce => jumpForce;
    public float DashCoolDown => dashCoolDown;
    public string[] UncontrollableAction => uncontrollableAction;
}
