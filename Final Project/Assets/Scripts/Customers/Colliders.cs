// Ignore Spelling: Colliders
using UnityEngine;

public class Colliders : MonoBehaviour
{
    [Header("Speed:")]
    [SerializeField] private float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Vector2? targetPosition = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
    private void FixedUpdate()
    {
        if (targetPosition == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 directionToTarget = (targetPosition.Value - rb.position).normalized;

        rb.velocity = directionToTarget * moveSpeed;

        float distanceToTarget = Vector2.Distance(rb.position, targetPosition.Value);
        if (distanceToTarget < 0.1f)
        {
            StopMoving();
        }
    }
    public void MoveTo(Vector2 destination)
    {
        this.targetPosition = destination;
    }
    public void StopMoving()
    {
        this.targetPosition = null;
    }
}