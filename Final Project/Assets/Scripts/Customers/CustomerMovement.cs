using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(CustomerAnimator))]
public class CustomerMovement : MonoBehaviour
{
    public event Action OnDestinationReached;

    //[Header("Obstacle Avoidance:")]
    //[SerializeField] private LayerMask obstacleLayerMask;
    //[SerializeField] private float detectionDistance = 1.0f;
    //[SerializeField] private float avoidanceStrength = 0.5f;

    [Header("Dynamic Movement Settings")]
    [SerializeField] private float moveForce = 75f;
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private float stoppingDistance = 0.2f;
    [SerializeField] [Range(0, 1)] private float stoppingSpeedFactor = 0.9f;

    [Header("Settings:")]
   // [SerializeField] private float moveSpeed = 4.0f;
    
    [Header("Animation:")]
    [SerializeField] public CustomerAnimator npcAnimator;
    [SerializeField] public SpriteRenderer sr;

    private Rigidbody2D rb;
    private Vector2? targetPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        npcAnimator = GetComponent<CustomerAnimator>();
        //sr = GetComponent<SpriteRenderer>();

        //----Dynamic---
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        rb.drag = 3f;
        rb.angularDrag = 5f;

        //----Kinematic----
        //rb.isKinematic = true;
        //rb.gravityScale = 0;
        //rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void FixedUpdate()
    {
        if (!targetPosition.HasValue)
        {
            rb.velocity *= stoppingSpeedFactor;
            if (rb.velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector2.zero;
            }
            return;
        }

        Vector2 currentPos = rb.position;
        if (Vector2.Distance(currentPos, targetPosition.Value) < stoppingDistance)
        //if (Vector2.Distance(currentPos, targetPosition.Value) < 0.05f)
        {
            StopMovementAndNotify();
            return;
        }

        //----Dynamic---
        Vector2 direction = (targetPosition.Value - currentPos).normalized;
        rb.AddForce(direction * moveForce);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        if (rb.velocity.magnitude > 0.1f)
        {
            npcAnimator.UpdateAnimationDirection(rb.velocity.normalized);
        }

        //----Kinematic----
        //Vector2 newPosition = Vector2.MoveTowards(currentPos, targetPosition.Value, moveSpeed * Time.fixedDeltaTime);
        //rb.MovePosition(newPosition);
        //Vector2 direction = (targetPosition.Value - currentPos).normalized;
        //npcAnimator.UpdateAnimationDirection(direction);

        //Vector2 direction = (targetPosition.Value - currentPos).normalized;

        //Vector2 directionToTarget = (targetPosition.Value - currentPos).normalized;
        //Vector2 finalDirection = directionToTarget;

        //RaycastHit2D hit = Physics2D.Raycast(currentPos, directionToTarget, detectionDistance, obstacleLayerMask);

        //if (hit.collider != null)
        //{
        //    Vector2 sidewaysDirection = Vector2.Perpendicular(directionToTarget).normalized;
        //    finalDirection = (directionToTarget + sidewaysDirection * avoidanceStrength).normalized;
        //}

        //rb.velocity = finalDirection * moveSpeed;
        //NPCAnimator.UpdateAnimationDirection(finalDirection);
    }
    public void MoveTo(Vector2 destination)
    {
        targetPosition = destination;
        npcAnimator.StartWalking();
    }
    private void StopMovementAndNotify()
    {
        targetPosition = null;
        //rb.velocity = Vector2.zero;
        npcAnimator.StopWalking();
        OnDestinationReached?.Invoke();
    }
}