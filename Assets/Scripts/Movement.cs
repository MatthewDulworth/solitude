using System;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour {
    // -----------------------------------------------------------------------------------------------------------------
    // Private Vars 
    // -----------------------------------------------------------------------------------------------------------------
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float defaultGravity;
    private float defaultDrag;

    // state 
    private bool canMove = true;
    private bool facingRight = true;
    private bool grounded;
    private bool isDashing;
    private Vector2 directionHeld;

    // input 
    private bool jumpRequested;
    private bool dashRequested;

    // ground detection
    private Vector2 groundDetectorSize;
    private Vector2 playerSize;

    // -----------------------------------------------------------------------------------------------------------------
    // Editor Variables 
    // -----------------------------------------------------------------------------------------------------------------
    [Header("Stats")] public float speed;
    public float jumpSpeed;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float dashSpeed;
    public float dashWait;
    public float dashDrag;

    [Header("Effects")] public float distBetweenAfterImages;

    [Header("Collision Detection")] public LayerMask groundLayer;
    public float groundDetectorHeight;


    // -----------------------------------------------------------------------------------------------------------------
    // Startup 
    // -----------------------------------------------------------------------------------------------------------------
    private void Awake() {
        playerSize = GetComponent<BoxCollider2D>().size;
        groundDetectorSize = new Vector2(playerSize.x, groundDetectorHeight);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.green;
        defaultGravity = rb.gravityScale;
        defaultDrag = rb.drag;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Input 
    // -----------------------------------------------------------------------------------------------------------------
    private void Update() {
        jumpRequested = (Input.GetKeyDown(KeyCode.K) || jumpRequested) && grounded;
        dashRequested = (Input.GetKeyDown(KeyCode.Space) || dashRequested);
        directionHeld = GetDirectionHeld();
    }

    private Vector2 GetDirectionHeld() {
        Vector2 direction = Vector2.zero;
        direction += Input.GetKey(KeyCode.W) ? Vector2.up : Vector2.zero;
        direction += Input.GetKey(KeyCode.D) ? Vector2.right : Vector2.zero;
        direction += Input.GetKey(KeyCode.S) ? Vector2.down : Vector2.zero;
        direction += Input.GetKey(KeyCode.A) ? Vector2.left : Vector2.zero;
        return direction;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Movement
    // -----------------------------------------------------------------------------------------------------------------
    private void FixedUpdate() {
        CheckGrounded();
        if (canMove) {
            if (HandleDash()){
                return;
            }
            HandleHorizontalMove();
            HandleJump();
            HandleJumpGravity();
        }
    }

    private void CheckGrounded() {
        Vector2 boxCenter = transform.position;
        boxCenter += Vector2.down * ((playerSize.y + groundDetectorSize.y) * 0.5f);
        grounded = Physics2D.OverlapBox(boxCenter, groundDetectorSize, 0, groundLayer) != null;
    }

    private void HandleHorizontalMove() {
        rb.velocity = new Vector2(directionHeld.x * speed, rb.velocity.y);
    }

    private bool HandleDash() {
        if (dashRequested) {
            Vector2 dashDir = directionHeld;
            if (dashDir.Equals(Vector2.zero)) {
                int facing = facingRight ? 1 : -1;
                dashDir = new Vector2(facing, 0);
            }
            
            rb.gravityScale = 0;
            rb.AddForce(dashSpeed * dashDir, ForceMode2D.Impulse);
            StartCoroutine(DashWait());
            dashRequested = false;
            return true;
        }
        return false;
    }

    private IEnumerator DashWait() {
        canMove = false;
        rb.gravityScale = 0;
        rb.drag = dashDrag;
        sr.color = Color.red;
        yield return new WaitForSeconds(dashWait);

        sr.color = Color.green;
        rb.drag = defaultDrag;
        rb.gravityScale = defaultGravity;
        canMove = true;
    }

    private void HandleJump() {
        if (jumpRequested) {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
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