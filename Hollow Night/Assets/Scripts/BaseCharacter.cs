using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    #region Variables
    // Components
    protected Rigidbody2D rb;
    protected Animator myAnimator;

    // Grounded
    [Header("Grounded")]
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform checkGround;
    [SerializeField] protected bool isGrounded;

    // Basic Movement
    protected float maxSpeed = 150f;
    protected float direction;
    protected bool facingRight = true;

    // Jump
    [Header("Jump")]
    [SerializeField] protected float jumpHeight = 1000;
    
    // Falling
    protected bool falling;
    #endregion


    #region Monos
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    public virtual void Update()
    {
        if (rb.velocity.y < 0f)
        {
            falling = true;
        }
        else
        {
            falling = false;
        }

        myAnimator.SetBool("Grounded", isGrounded);
        myAnimator.SetBool("Falling", falling);
    }

    public virtual void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(checkGround.position, 0.1f, whatIsGround);
    }
    #endregion


    #region Mechanics
    protected void Move()
    {
        rb.velocity = new Vector2(direction * maxSpeed * Time.fixedDeltaTime, rb.velocity.y);
        myAnimator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    protected void Flip()
    {
        if (direction > 0 && !facingRight || direction < 0 && facingRight)
        {
            facingRight = !facingRight;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }

    protected void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight * Time.fixedDeltaTime);
    }
    #endregion


    #region SubMechanics
    protected virtual void HandleMovement()
    {
        Move();
        Flip();
    }

    protected abstract void HandleJump();
    #endregion
}
