using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    Animator spriteAnimator;
    public GameObject alertThingy;
    private GameObject createdChild;
    private bool showingAlert;
    private float _alertTime;

    private void Start()
    {
        spriteAnimator = transform.GetChild(0).GetComponent<Animator>();
        showingAlert = false;
        _alertTime = 0f;
    }

    public void ShowAlert()
    {
        if (!showingAlert)
        {
            EnemyPatrolBase eBase = gameObject.GetComponent<EnemyPatrolBase>();
            Vector3 alertPosition = new Vector3(0, eBase.spriteHeight+0.5f, 0) + transform.position + transform.right * 0.3f;
            createdChild = Instantiate(alertThingy, alertPosition, transform.GetChild(0).transform.rotation);
            showingAlert = true;
        }
    }

    private void DestroyAlert()
    {
        Destroy(createdChild);
    }

    private void Update()
    {
        if (showingAlert)
        {
            _alertTime += Time.deltaTime;
            if (_alertTime > 1f)
            {
                DestroyAlert();
                showingAlert = false;
                _alertTime = 0f;
            }
        }
    }

    public void WizardTrigger(string trigger_name)
    {
        Debug.Log(trigger_name);
        spriteAnimator.SetTrigger(trigger_name);
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
            case State.StateTypes.Recovering:
                spriteAnimator.SetBool("isRecovering", isActive);
                break;
            case State.StateTypes.Alerted:
                spriteAnimator.SetBool("isAlerted", isActive);
                break;
            case State.StateTypes.Dead:
                spriteAnimator.SetBool("isDead", isActive);
                break;
            default:
                spriteAnimator.SetBool("isIdle", isActive);
                break;
        }
    }
}
