using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Movement : MonoBehaviour {
    // private vars
    private Rigidbody2D rb;
    private int dir;
    private bool jump;
    private Vector2 boxSize;
    private Vector2 playerSize;

    // editor vars
    public float speed = 10;
    public float jumpVelocity = 5;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float groundDepth = 0.05f;
    public bool grounded;
    public LayerMask groundLayer;

    private void Awake() {
        playerSize = GetComponent<BoxCollider2D>().size;
        boxSize = new Vector2(playerSize.x, groundDepth);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        // update direction 
        dir = 0;
        dir += Input.GetKey(KeyCode.A) ? -1 : 0;
        dir += Input.GetKey(KeyCode.D) ? 1 : 0;

        // update jump
        jump = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);

        if (jump && grounded) {
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            jump = false;
        }
        else {
            Vector2 boxCenter = (Vector2) transform.position + Vector2.down * ((playerSize.y + boxSize.y) * 0.5f);
            grounded = Physics2D.OverlapBox(boxCenter, boxSize, 0, groundLayer) != null;
        }

        HandleJumpGravity();
    }

    private void HandleJumpGravity() {
        if (rb.velocity.y < 0) {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
            rb.gravityScale = lowJumpMultiplier;
        }
        else {
            rb.gravityScale = 1f;
        }
    }
}