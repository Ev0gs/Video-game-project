using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private float Speed;
    public float moveSpeed;
    public float crouchMoveSpeed;
    private SpriteRenderer sr;

    public float jumpForce;
    private float moveInput;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public float jumpStartTime;
    private float jumpTime;
    private bool isJumping;

    private bool isCrouching;

    public Animator animator;

    public static PlayerMovement instance;

    public void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Player_Health dans la scène");
            return;
        }
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        FaceMoveDirection();
        Jump();
        AnimationState();
    }

    void FixedUpdate()
    {
        if (isCrouching)
        {
            Speed = crouchMoveSpeed;
        }
        else if (!isCrouching)
        {
            Speed = moveSpeed;
        }
        rb.velocity = new Vector2(moveInput * Speed, rb.velocity.y);

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded == true && Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJumping = true;
            //animator.SetBool("IsJumping", true);
            jumpTime = jumpStartTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.UpArrow) && isJumping == true)
        {
            if (jumpTime > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                //animator.SetBool("IsJumping", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
            //animator.SetBool("IsJumping", false);
        }
    }

    void FaceMoveDirection()
    {
        if (moveInput > 0)
        {
            sr.flipX = false;
        }
        else if (moveInput < 0)
        {
            sr.flipX = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
    }

    void AnimationState()
    {
        //Jump animation
        if(rb.velocity.y > 1)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
        }
        if(rb.velocity.y < -1)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);
        }
        if(rb.velocity.y == 0)
        {
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
        }
        //Crouch Animation
        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("Crouch", true);
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            animator.SetBool("Crouch", false);
            isCrouching = false;
        }
    }
}