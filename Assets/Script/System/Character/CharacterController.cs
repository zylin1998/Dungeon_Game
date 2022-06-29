using System.Collections;
using UnityEngine;
using CustomInput;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerInput))]
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterDetailAsset characterDetail;

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float dashCoolDown;
    [SerializeField]
    private string[] uncontrollableAction;

    private float maxJumpHold = 0.3f;
    private int maxJumpCount = 2;
    private int jumpCount;

    private float isCollision;
    private bool isGround;
    private bool isFlip;

    private PlayerInput playerInput;

    private Animator animator;
    private Rigidbody2D rigid;
    private CapsuleCollider2D capsule;

    public float VerticalVelocity => rigid.velocity.y;
    public float IsCollision { get => isCollision; set => isCollision = value; }
    public bool IsFlip { get => isFlip; set => isFlip = value; }
    public bool IsGround { get => isGround; set => isGround = value; }

    #region is now uncontrollable

    private bool uncontrollable
    {
        get 
        {
            foreach (string action in uncontrollableAction) 
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(action)) { return true; }
            }

            return false;
        }
    }

    #endregion

    private void Awake()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        playerInput = this.GetComponent<PlayerInput>();
        capsule = this.GetComponent<CapsuleCollider2D>();
        animator = this.GetComponentInChildren<Animator>();

        CharacterInitialize();
    }

    private void Update()
    {
        if (uncontrollable) { return; }

        Move();

        Jump();

        Dash();

        Attack();
    }

    #region Basic Movement

    private void Move()
    {
        float horizontal = playerInput.horizontal;
        float vertical = playerInput.vertical;

        if (vertical >= 0 && horizontal == 0) 
        {
            if (isGround) { animator.Play("Idle"); } 
        }

        if (horizontal > 0) { isFlip = true; }
        if (horizontal < 0) { isFlip = false; }

        Flip(isFlip);

        if (vertical < 0 && isGround) 
        {
            animator.Play("Sit");
            return;
        }

        float speed = horizontal * walkSpeed * Time.deltaTime;

        transform.transform.Translate(CheckSpeed(speed), 0f, 0f);

        if (horizontal != 0 && isGround) { animator.Play("Run"); }
    }

    private float jumpPress = 0f;

    private void Jump()
    {
        if (isGround) { jumpCount = 0; }

        if (!isGround && jumpCount > maxJumpCount) { return; }

        if (playerInput.jump)
        {
            animator.Play("Jump");

            isGround = false;
            
            jumpCount++;

            if (jumpCount <= maxJumpCount)
            { 
                jumpPress = 0f;

                rigid.AddForce(rigid.velocity = Vector2.up * jumpForce * 0.4f);
            }

            return;
        }

        if (!isGround && playerInput.jumpHold && jumpPress <= maxJumpHold) 
        {
            jumpPress += Time.deltaTime;

            if (jumpPress >= 0.1f) { rigid.AddForce(rigid.velocity = Vector2.up * jumpForce * 0.55f); }
        }
    }

    private float dashPress = 0f;

    private void Dash() 
    {
        var passTime = Time.realtimeSinceStartup - dashPress;

        if (dashPress != 0 && passTime < dashCoolDown) { return; }

        if (playerInput.dash)
        {
            dashPress = Time.realtimeSinceStartup;

            animator.Play("Dash");

            onPlayingCallBack = new OnAnimatorPlaying(Dashing);
            StartCoroutine(AnimatorPlaying("Dash"));
        }
    }

    private void Attack() 
    {
        if (!isGround) { return; }

        if (playerInput.attack)
        {
            animator.Play("Attack");
        }
    }

    private float CheckSpeed(float speed) 
    {
        if( isCollision < 0 && speed < 0) { return 0f; }

        if (isCollision > 0 && speed > 0) { return 0f; }

        return speed;
    }

    #endregion

    #region Action Event Invoke

    private delegate void OnAnimatorPlaying();
    private OnAnimatorPlaying onPlayingCallBack;

    private IEnumerator AnimatorPlaying(string name) 
    {
        while(!animator.GetCurrentAnimatorStateInfo(0).IsName(name)) { yield return null; }

        while (animator.GetCurrentAnimatorStateInfo(0).IsName(name)) 
        {
            onPlayingCallBack.Invoke();

            yield return null;
        }
    }

    private void Dashing() 
    {
        float dashTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float speed = (isFlip ? 1 : -1) * dashSpeed * Time.deltaTime;

        if (dashTime >= 0.2f && dashTime <= 0.8f)
        {
            rigid.velocity = Vector2.zero;
            transform.transform.Translate(CheckSpeed(speed), 0f, 0f);
        }
    }

    #endregion

    #region Initialize

    private void CharacterInitialize() 
    {
        if (characterDetail == null) { return; }

        walkSpeed = characterDetail.WalkSpeed;
        dashSpeed = characterDetail.DashSpeed;
        jumpForce = characterDetail.JumpForce;
        dashCoolDown = characterDetail.DashCoolDown;
        uncontrollableAction = characterDetail.UncontrollableAction;
    }

    #endregion

    public void Flip(bool flip) 
    {
        var flipX = flip ? -1 : 1;

        transform.localScale = new Vector3(flipX, 1f, 1f);
    }
}
