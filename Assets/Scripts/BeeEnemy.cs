using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEnemy : MonoBehaviour
{
    public float moveSpeed = 2f; 
    public float patrolRange = 4f; 

    private SpriteRenderer beeRenderer;
    public GameObject beeEnemy;

    private Rigidbody2D rb;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        beeRenderer = beeEnemy.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Patrol behavior: move horizontally back and forth
        if (movingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            if (transform.position.x >= patrolRange)
            {
                Flip();
            }
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            if (transform.position.x <= -patrolRange)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        // Reverse the direction of movement
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        beeRenderer.flipX = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collides with the bee
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check collision direction
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

            if (collision.transform.Dot(transform, Vector2.down))
            {
                Destroy(beeEnemy, 0.1f);
            }
            else 
            {
                player.Death();
            }
        }
    }
}
