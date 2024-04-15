using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    // Layermask for detecting platforms
    [SerializeField] private LayerMask platformLayerMask;

    // Reference to player
    public GameObject player;

    // Rigidbody2D
    private Rigidbody2D rb;

    // Reference to camera
    private Camera m_camera;

    // Reference to sprite renderer
    private SpriteRenderer m_spriteRenderer;

    // Reference to animator
    private Animator m_animator;

    // Movement and input variables
    private Vector2 velocity;
    private Vector2 position;
    private float moveInput;
    private float lastInput;

    // Movement parameters
    public float moveSpeed = 5f;
    public float deceleration = 10f;
    public float acceleration = 1.0f;

    // Jump parameters
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    private float downForce;

    // Jump force and gravity properties
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2);
    public bool jumping { get; private set; }
    public bool grounded { get; private set; }


    private bool isDead = false;

    private void Awake()
    {
        // Initialize references
        rb = GetComponent<Rigidbody2D>();
        m_spriteRenderer = player.GetComponent<SpriteRenderer>();
        m_animator = player.GetComponent<Animator>();
        m_camera = Camera.main;
    }

    private void Update()
    {
        HorizontalMovement();

        if (isGrounded())
        {
            Jump();
        }

        ApplyGravity();
        HandleAnimations();
    }

    private void HorizontalMovement()
    {
        // Obtain horizontal input
        moveInput = Input.GetAxis("Horizontal");

        // Record the last input for sprite flipping
        if (moveInput != 0)
        {
            lastInput = moveInput;
        }

        // Perform raycast to check for wall collision
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * moveInput, 0.51f, platformLayerMask);

        // Draw the ray for visualization
        Debug.DrawRay(transform.position, Vector2.right * moveInput * 0.51f, Color.red);

        // If the ray hits a wall, set velocity.x to 0
        if (hit.collider != null)
        {
            velocity.x = 0f;
        }
        else
        {
            // Adjust velocity based on input and moveSpeed
            velocity.x = Mathf.MoveTowards(velocity.x, moveInput * moveSpeed, moveSpeed * Time.deltaTime);

  

            // If moving in the opposite direction of current velocity, set velocity.x to 0
            if (Mathf.Sign(moveInput) != Mathf.Sign(velocity.x))
            {
                velocity.x = 0f;
            }
            else
            {
                // Adjust velocity based on input and moveSpeed
                velocity.x = Mathf.MoveTowards(velocity.x, moveInput * moveSpeed, Time.deltaTime * acceleration);
            }

        }
    }

    private void Jump()
    {
        // Check that vertical velocity is not negative
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        // Check for the jump input and initiate a jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpForce;
            jumping = true;
            AudioManager.instance.PlayJumpSound();
            Debug.Log("Jump");
        }
    }

    private void ApplyGravity()
    {
        // Determine if player is falling or if space is held
        bool falling = velocity.y < 0f || !Input.GetKey(KeyCode.Space);

        // Apply downforce based on falling status
        downForce = falling ? 2f : 1f;

        // Update vertical velocity
        velocity.y += gravity * downForce * Time.deltaTime;

        // Ensure a minimum vertical velocity
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void FixedUpdate()
    {
        // Move the player
        position = rb.position;
        position += velocity * Time.fixedDeltaTime;

        // Apply the new position to the Rigidbody 
        rb.MovePosition(position);
    }


    private bool isGrounded()
    {
        return transform.Find("GroundCheck").GetComponent<GroundCheck>().isGrounded;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player collides with a bounce platform
        if (collision.gameObject.layer == LayerMask.NameToLayer("BouncePlatform"))
        {
            // If player jumps on an enemy
            if (transform.Dot(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce * 1.25f;
                jumping = true;
                AudioManager.instance.PlayJumpSound();
            }
            else if (transform.Dot(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (transform.Dot(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce * 1.25f;
                jumping = true;
                AudioManager.instance.PlayJumpSound();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Flag"))
        {
            GameManager.Instance.Victory();
        }
    }

    private void HandleAnimations()
    {
        if (isGrounded())
        {
            m_animator.SetBool("Jumping", false);
        }
        else
        {
            m_animator.SetBool("Jumping", true);
        }

        if (lastInput < 0)
        {
            m_spriteRenderer.flipX = false;
        }
        else
        {
            m_spriteRenderer.flipX = true;
        }
    }

    public void Death()
    {
        isDead = true;
        // play sound effect
        GameManager.Instance.ResetLevel(0.2f);
    }
}
