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
        jump = (Input.GetKeyDown(KeyCode.Space) || jump) && grounded;
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);

        if (jump) {
            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            jump = false;
        }
        else {
            Vector2 boxCenter = (Vector2) transform.position + Vector2.down * ((playerSize.y + boxSize.y) * 0.5f);
            grounded = Physics2D.OverlapBox(boxCenter, boxSize, 0, groundLayer) != null;
        }

        HandleJumpGravity();
    }

    /**
     * Handles the behavior for long and short jumps as well as fall speed.
     */
    private void HandleJumpGravity() {
        if (rb.velocity.y < 0) {
            // player is falling 
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
            // player is doing a low jump
            rb.gravityScale = lowJumpMultiplier;
        }
        else {
            // player is doing a high jump or not jumping 
            rb.gravityScale = 1f;
        }
    }
}