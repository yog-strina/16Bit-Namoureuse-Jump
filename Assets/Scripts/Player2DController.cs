using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Player2DController : MonoBehaviour {

    public RuntimeAnimatorController animatorController;
    public float jumpMax = 3f;
    public float jumpSpeed = 2f;
    public float moveSpeed = 7f;
    public float fallSpeed = 2f;
    public float invulnerabilityTime = 3f;
    public GroundCheck groundCheck;
    public bool isWalking;
    public bool isIdle;
    public bool isMeleeAttacking;
    public bool isGrounded;
    public bool isFlipped;
    public bool isInvulnerable = false;
    public bool isBlinking = false;
    public bool jumps;
    public List<FollowerController> followers;

    private float horizontalAxis;
    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidBody2D.gravityScale = fallSpeed;
        animator.runtimeAnimatorController = animatorController;
    }

    // Update is called once per frame
    void Update()
    {
        //If Invulnerable then Blink
        if (isInvulnerable && !isBlinking) {
            StartCoroutine(Blink());
            isBlinking = true;
        }
        else if (!isInvulnerable && isBlinking) {
            StopCoroutine(Blink());
            isBlinking = false;
        }

        //Flags and variables
            //Variables
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        jumps = Input.GetKeyDown(KeyCode.Space);
            // Control Flags
        isFlipped = horizontalAxis < 0;
        isWalking = horizontalAxis != 0;
        isIdle = horizontalAxis == 0;
        isGrounded = groundCheck.isGrounded;
        isMeleeAttacking = Input.GetKeyDown(KeyCode.E);

        //Actions
            //Jump
        if (jumps && isGrounded) {
            isGrounded = false;
            rigidBody2D.AddForce(new Vector2(0, jumpMax), ForceMode2D.Impulse);
        }
        
            //Movements
        if (horizontalAxis != 0) {
            foreach (FollowerController follower in followers)
                follower.FlipSprite(isFlipped);
            spriteRenderer.flipX = isFlipped;
            transform.position += new Vector3(horizontalAxis * moveSpeed * Time.deltaTime, 0, 0);
        }

            //Attack
        /*TODO: Logic d'attack*/

        //Animations
        if (!isGrounded) {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsWalking", false);
        }
        else if (isWalking) {
            foreach (FollowerController follower in followers) {
                follower.SetWalking(true);
            }
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsJumping", false);
        }
        else if (isIdle) {
            foreach (FollowerController follower in followers) {
                follower.SetWalking(false);
            }
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsJumping", false);
        }
        animator.SetBool("IsAttacking", isMeleeAttacking);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("trigger enter");
        //Get touched by ennemy
        if (other.gameObject.tag == Tags.BADDY && !isInvulnerable) {
            Debug.Log("is ennemy");
            StartCoroutine(GetHurt());
        }
    }

    private IEnumerator GetHurt() {
        Debug.Log("got hurt");
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }

    private IEnumerator Blink() {
        Debug.Log("do the blinks");
        for (float i = invulnerabilityTime; i > 0; i -= Time.deltaTime) {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(invulnerabilityTime / 20f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(invulnerabilityTime / 20f);
        }
    }
}