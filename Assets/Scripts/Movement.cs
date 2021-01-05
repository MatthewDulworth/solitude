using UnityEngine;

public class Movement : MonoBehaviour {
    // private vars
    private Rigidbody2D rb;
    private int dir;
    private bool jumpRequested;
    private Vector2 boxSize;
    private Vector2 playerSize;
    private float defaultGravity;
    public bool facingRight = true;

    // editor vars
    public float speed;
    public float jumpVelocity;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float groundDepth;
    public bool grounded;
    public LayerMask groundLayer;

    // dash 
    private bool dashRequested;
    public float distBetweenAfterImages;

    private void Awake() {
        playerSize = GetComponent<BoxCollider2D>().size;
        boxSize = new Vector2(playerSize.x, groundDepth);
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    private void Update() {
        // update direction 
        dir = 0;
        dir += Input.GetKey(KeyCode.A) ? -1 : 0;
        dir += Input.GetKey(KeyCode.D) ? 1 : 0;

        jumpRequested = (Input.GetKeyDown(KeyCode.K) || jumpRequested) && grounded;
        dashRequested = (Input.GetKeyDown(KeyCode.Space) || dashRequested);
    }

    private void FixedUpdate() {
        CheckGrounded();
        HandleDash();
        HandleHorizontalMove();
        HandleJump();
        HandleJumpGravity();
    }

    private void CheckGrounded() {
        Vector2 boxCenter = (Vector2) transform.position + Vector2.down * ((playerSize.y + boxSize.y) * 0.5f);
        grounded = Physics2D.OverlapBox(boxCenter, boxSize, 0, groundLayer) != null;
    }

    private void HandleHorizontalMove() {
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);
    }

    private void HandleDash() {
        if (dashRequested) {
        }
    }

    private void HandleJump() {
        if (jumpRequested) {
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            jumpRequested = false;
        }
    }

    // Handles the behavior for long and short jumps as well as fall speed.
    private void HandleJumpGravity() {
        if (rb.velocity.y < 0) {
            // player is falling 
            rb.gravityScale = fallMultiplier * defaultGravity;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
            // player is doing a low jump
            rb.gravityScale = lowJumpMultiplier * defaultGravity;
        }
        else {
            // player is doing a high jump or not jumping 
            rb.gravityScale = defaultGravity;
        }
    }
}