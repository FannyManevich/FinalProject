using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputChannel inputChannel;

    public Transform leftWall;
    public Transform rightWall;
    public Transform bottomWall;
    public Transform topWall;

    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (inputChannel != null)
        {
            inputChannel.OnMoveEvent += OnMove;
        }
        else
        {
            Debug.LogError("InputChannel is not assigned in PlayerMovement!");
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveSpeed * moveInput;

        Vector2 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, leftWall.position.x, rightWall.position.x);
        clampedPos.y = Mathf.Clamp(clampedPos.y, bottomWall.position.y, topWall.position.y);
        transform.position = clampedPos;
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
