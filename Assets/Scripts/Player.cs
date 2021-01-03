using System;
using UnityEngine;

/*
 * handles player movement
 */
public class Player : MonoBehaviour {
    // editor vars
    [SerializeField] private float speed = 10;
    [SerializeField] private Vector2 jumpHeight = new Vector2(0, 1000f);
    [SerializeField] private bool facingRight = true;
    [SerializeField] private LayerMask groundLayer;

    // private vars
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D collider;
    private bool isGrounded = true;

    // animation params
    private static readonly int Running = Animator.StringToHash("isRunning");
    private static readonly int Grounded = Animator.StringToHash("isGrounded");
    private static readonly int JumpTrigger = Animator.StringToHash("jumpTrigger");

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() {
        isGrounded = true;
        //CheckGrounded();
        HandleMovement();
    }

    // handles movement
    private void HandleMovement() {
        
        // horizontal
        int x = 0;
        if (Input.GetKey(KeyCode.A)) {
            x += -1;
        }

        if (Input.GetKey(KeyCode.D)) {
            x += 1;
        }

        // vertical 
        if (Input.GetKeyDown(KeyCode.J) && isGrounded) {
            Debug.Log("jump");
            rb.AddForce(jumpHeight, ForceMode2D.Impulse);
        }
        
        rb.velocity = new Vector2(x * speed, rb.velocity.y);
        MoveAnimation(x);
    }
    
    // handles grounded animations
    private void MoveAnimation(int x) {
        // direction facing 
        if (x == 1 && !facingRight || x == -1 && facingRight) {
            FlipX();
        }

        // run 
        if (x == 0 && animator.GetBool(Running)) {
            animator.SetBool(Running, false);
        }
        else if (x != 0 && !animator.GetBool(Running)) {
            animator.SetBool(Running, true);
        }
        
        // // jump
        // if (isGrounded != animator.GetBool(Grounded)) {
        //     animator.SetBool(Grounded, isGrounded);
        // }
    }

    private bool CheckGrounded() {
        bool ground = collider.IsTouchingLayers(groundLayer);
        Debug.Log(ground);
        return ground;
    }

    // flips the x-axis
    private void FlipX() {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
        facingRight = !facingRight;
    }
}