using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    [SerializeField] private float Speed;

    private CustomerBehavior CustomerBehavior;

    private Animator animator;
    private SpriteRenderer sr;
    private float step;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        step = Speed * Time.deltaTime;
        CustomerBehavior = GetComponent<CustomerBehavior>();
    }

    public void MoveToPointXFirst(Vector2 PointToWalkTo, bool ChangeStateAtEnd)
    {
        if (transform.position.x != PointToWalkTo.x)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isWalkingSide", true);
            sr.flipX = (transform.position.x - PointToWalkTo.x > 0);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(PointToWalkTo.x, transform.position.y, 0), step);
        }
        else if (transform.position.y != PointToWalkTo.y)
        {
            sr.flipX = false;
            animator.SetBool("isWalkingSide", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isWalkingDown", (transform.position.y - PointToWalkTo.y < 0));
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, PointToWalkTo.y, 0), step);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalkingSide", false);
            if (ChangeStateAtEnd)
            {
                CustomerBehavior.NextState();
            }
        }
    }
    public void MoveToPointYFirst(Vector2 PointToWalkTo, bool ChangeStateAtEnd)
    {
        if (transform.position.y != PointToWalkTo.y)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isWalkingDown", (transform.position.y - PointToWalkTo.y < 0));
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, PointToWalkTo.y, 0), step);
        }
        else if (transform.position.x != PointToWalkTo.x)
        {
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isWalkingSide", true);
            sr.flipX = (transform.position.x - PointToWalkTo.x > 0);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(PointToWalkTo.x, transform.position.y, 0), step);
        }
        else
        {
            sr.flipX = false;
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalkingSide", false);
            if(ChangeStateAtEnd) 
            {
                CustomerBehavior.NextState();
            }
        }
    }
}
