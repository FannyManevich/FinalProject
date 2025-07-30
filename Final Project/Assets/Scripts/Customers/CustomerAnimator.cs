using UnityEngine;
public class CustomerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void StartWalking()
    {
        animator.SetBool("isWalking", true);
    }
    public void StopWalking()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isWalkingSide", false);
        animator.SetBool("isWalkingDown", false);
    }
    public void UpdateAnimationDirection(Vector2 moveDirection)
    {
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            animator.SetBool("isWalkingSide", true);
            spriteRenderer.flipX = moveDirection.x < 0;
        }
        else
        {
            animator.SetBool("isWalkingSide", false);
            animator.SetBool("isWalkingDown", moveDirection.y < 0);
        }
    }
}