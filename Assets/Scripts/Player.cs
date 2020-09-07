using UnityEngine;

/*
 * controls player movement
 */
public class Player : MonoBehaviour {
    // editor vars
    [SerializeField] private float speed = 10;
    [SerializeField] private bool facingRight = true;

    // private vars
    private Rigidbody2D rb;
    private Animator animator;

    // animation params
    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        Movement();
    }

    // handles horizontal movement
    private void Movement() {
        int x = 0;
        if (Input.GetKey(KeyCode.A)) {
            x += -1;
        }

        if (Input.GetKey(KeyCode.D)) {
            x += 1;
        }

        rb.velocity = new Vector2(x * speed, rb.velocity.y);
        MoveAnimation(x);
    }

    // handles grounded animations
    private void MoveAnimation(int x) {
        if (x == 1 && !facingRight || x == -1 && facingRight) {
            FlipX();
        }

        if (x == 0 && animator.GetBool(IsRunning)) {
            animator.SetBool(IsRunning, false);
        }
        else if (x != 0 && !animator.GetBool(IsRunning)) {
            animator.SetBool(IsRunning, true);
        }
    }

    // flips the x-axis
    private void FlipX() {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
        facingRight = !facingRight;
    }
}