using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    Animator spriteAnimator;
    private void Start()
    {
        spriteAnimator = transform.GetChild(0).GetComponent<Animator>();
    }
    public void SetParameter(State.StateTypes animationState, bool isActive)
    {
        switch (animationState)
        {
            case State.StateTypes.Patrolling:
                spriteAnimator.SetBool("isPatrolling", isActive);
                break;
            case State.StateTypes.Attacking:
                spriteAnimator.SetBool("isAttacking", isActive);
                break;
            default:
                spriteAnimator.SetBool("isIdle", isActive);
                break;
        }
    }
}
