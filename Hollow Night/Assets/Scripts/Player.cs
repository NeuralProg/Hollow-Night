using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
    // Variables
    private int jumpCount = 1;


    # region Monos
    public override void Start()
    {
        base.Start();
        maxSpeed = 200f;
    }

    public override void Update()
    {
        base.Update();
        if (isGrounded)
        {
            jumpCount = 1;
            myAnimator.ResetTrigger("Jump");
        }

        if (falling)
        {
            myAnimator.ResetTrigger("Jump");
        }

        HandleJump();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        direction = Input.GetAxisRaw("Horizontal");
    }
    #endregion


    #region Functions
    protected override void HandleJump()
    {
        var jumpInput = Input.GetButtonDown("Jump");
        var jumpInputRelease = Input.GetButtonUp("Jump");

        if (jumpInput && jumpCount > 0)
        {
            Jump();
            jumpCount--;
            myAnimator.SetTrigger("Jump");
        }

        if (jumpInputRelease && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            myAnimator.ResetTrigger("Jump");
        }
    }
    #endregion
}
