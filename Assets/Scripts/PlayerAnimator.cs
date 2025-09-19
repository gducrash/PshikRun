using DG.Tweening.Core.Easing;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public GameManager gameManager;

    void Update()
    {
        animator.SetFloat("speedMultiplier", gameManager.speedMultiplier);
    }

    public void TriggerJump()
    {
        animator.ResetTrigger("Grounded");
        animator.SetTrigger("Jump");
    }

    public void TriggerGrounded()
    {
        animator.ResetTrigger("Jump");
        animator.SetTrigger("Grounded");
    }
}
