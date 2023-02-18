using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
    // Jump
    private int jumpCount = 1;
    private bool jumpInput;
    private bool jumpInputRelease;

    // Dash
    [SerializeField] private float dashForce = 200f;
    private bool canDash = false;
    private bool isDashing = false;
    private float dashCooldown;

    // Wall
    [Header("Walled")]
    [SerializeField] protected LayerMask whatIsWall;
    [SerializeField] protected Transform checkWall;
    [SerializeField] private bool isWalled;
    [SerializeField] private float wallFallingSpeed = 30f;
    private bool isJumpingOffWall = false;
    private float jumpOffWallCooldown;


    # region Monos
    public override void Start()
    {
        base.Start();
        maxSpeed = 200f;
    }

    public override void Update()
    {
        base.Update();

        direction = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        jumpInputRelease = Input.GetButtonUp("Jump");

        if (isGrounded || isWalled)
        {
            ResetMechanics();
        }

        if (falling)
        {
            myAnimator.ResetTrigger("Jump");
        }

        myAnimator.SetBool("Walled", isWalled);
        HandleJump();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!isDashing && !isJumpingOffWall)
        {
            HandleMovement();
        }

        bool checkWalled = Physics2D.OverlapCircle(checkWall.position, 0.275f, whatIsWall);
        if (checkWalled == true && isGrounded == false)
        {
            isWalled = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * wallFallingSpeed * Time.fixedDeltaTime);
        }
        else
        {
            isWalled = false;
        }

        if (isDashing)
        {
            rb.velocity = new Vector2(dashForce * transform.localScale.x * Time.fixedDeltaTime, rb.velocity.y);
            dashCooldown -= Time.fixedDeltaTime;
            if (dashCooldown <= 0)
            {
                isDashing = false;
                dashCooldown = 0.2f;
            }
        }

        if (isJumpingOffWall)
        {
            rb.velocity = new Vector2(120 * transform.localScale.x * Time.fixedDeltaTime, 220 * Time.fixedDeltaTime);
            jumpOffWallCooldown -= Time.fixedDeltaTime;
            if (jumpOffWallCooldown <= 0)
                isJumpingOffWall = false;
        }

        
        HandleDash();
    }
    #endregion


    #region Functions
    protected override void HandleJump()
    {
        if (jumpInput && jumpCount > 0)
        {
            if (isWalled)
            {
                Jump();
                JumpWall();

                facingRight = !facingRight;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
            else
            {
                Jump();
                jumpCount--;
            }
            print("test1");
            myAnimator.SetTrigger("Jump");
        }

        if (jumpInputRelease && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            myAnimator.ResetTrigger("Jump");
            print("test2");
        }
    }

    private void JumpWall()
    {
        isJumpingOffWall = true;
        jumpOffWallCooldown = 0.15f;
    }

    private void HandleDash()
    {
        var dashInput = Input.GetButtonDown("Dash");

        if (dashInput && canDash)
        {
            isDashing = true;
            dashCooldown = 0.2f;
            canDash = false;
        }
    }

    private void ResetMechanics()
    {
        jumpCount = 1;
        canDash = true;
        //myAnimator.ResetTrigger("Jump");
    }
    #endregion
}
