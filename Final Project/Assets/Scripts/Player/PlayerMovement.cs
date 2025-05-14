using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputChannel inputChannel;

    // public Transform leftWall;
    // public Transform rightWall;
    // public Transform bottomWall;
    // public Transform topWall;

    public Animator animator;
    public SpriteRenderer sr;

    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (inputChannel != null)
        {
            inputChannel.OnMoveEvent += OnMove;
        }
        else
        {
            Debug.LogError("InputChannel is not assigned in PlayerMovement!");
        }
    }
    private void Update()
    {
        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isWalking", true);

            if (moveInput.x > 0.5)
            {
                animator.SetBool("isWalkingSide", true);
                sr.flipX = false;
            }
            else if (moveInput.x < -0.5)
            {
                animator.SetBool("isWalkingSide", true);
                sr.flipX = true;
            }
            else
            {
                animator.SetBool("isWalkingSide", false);
                sr.flipX = false;
            }

            if (moveInput.y > 0.5)
            {
                animator.SetBool("isWalkingBack", true);
            }
            else
            {
                animator.SetBool("isWalkingBack", false);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
    void FixedUpdate()
    {
        

        rb.velocity = moveSpeed * moveInput;

       // Vector2 clampedPos = transform.position;
       // clampedPos.x = Mathf.Clamp(clampedPos.x, leftWall.position.x, rightWall.position.x);
       // clampedPos.y = Mathf.Clamp(clampedPos.y, bottomWall.position.y, topWall.position.y);
       // transform.position = clampedPos;
    }

    public void OnMove(Vector2 input)
    {
        moveInput = input;
        Debug.Log("Move input: " + moveInput);
    }

    private void OnDestroy()
    {
        if (inputChannel != null)
        {
            inputChannel.OnMoveEvent -= OnMove;
        }
    }

}
