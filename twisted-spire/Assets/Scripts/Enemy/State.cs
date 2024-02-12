using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State { 
    public enum StateTypes
    {
        Idle,
        Patrolling,
        Alerted,
        Attacking,
        Recovering,
    }

    protected abstract void OnStateEnd(StateTypes nextState);
    protected abstract void OnStateStart(StateMachine sm);
    protected abstract void OnStateUpdate();

    public void UpdateState()
    {
        OnStateUpdate();
    }

    public void StartState(StateMachine stateMachine)
    {
        OnStateStart(stateMachine);
    }

    public void EndState(StateTypes nextState)
    {
        OnStateEnd(nextState);
    }
}

public class IdleState : State
{
    StateMachine sm;
    KinematicController km;
    bool isActive = true;
    public float idleTime;
    float internalTimer;
    protected override void OnStateEnd(StateTypes nextState)
    {
        EnemyAnimationController animationController = sm.GetAnimator();
        animationController.SetParameter(StateTypes.Idle, false);
        sm.SetState(nextState);
    }

    protected override void OnStateStart(StateMachine sm)
    {
        this.sm = sm;
        km = sm.GetComponent<KinematicController>();
        EnemyAnimationController animationController = sm.GetAnimator();
        animationController.SetParameter(StateTypes.Idle, true);
        sm.SetupIdle(this);
        internalTimer = 0.0f;
    }

    protected override void OnStateUpdate()
    {
        // Might be idling because this enemy is not active. I.e. it has spawned but no trigger has woken it up
        if (!isActive)
        {
            return;
        }

        // Creatures can go into attack mode from idle
        // Test for attack state
        if (sm.ShouldAttack())
        {
            EndState(StateTypes.Alerted);
        }

        // Creatures idle for some time, then go back to patrolling
        internalTimer += Time.deltaTime;
        if (internalTimer > idleTime)
        {   
            EndState(StateTypes.Patrolling);
        }
    }
}

public class PatrollingState : State
{
    public Vector3 point1;
    public Vector3 point2;
    private Vector3 currentTarget;
    public float tolerance;
    StateMachine sm;
    KinematicController km;

    protected override void OnStateEnd(StateTypes nextState)
    {
        km.ResetCleanup();
        EnemyAnimationController animationController = sm.GetAnimator();
        animationController.SetParameter(StateTypes.Patrolling, false);
        sm.SetState(nextState);
    }

    protected override void OnStateUpdate()
    {
        // Test for attack state
        if (sm.ShouldAttack())
        {
            EndState(StateTypes.Alerted);
        }

        // Move towards the current target
        km.MoveTowardsTarget();
        km.ResetHeight();
        if (km.DestinationWithinTolerance(sm.gameObject.transform.position, tolerance))
        {
            
            EndState(StateTypes.Idle);
        }
    }

    protected override void OnStateStart(StateMachine sm)
    {
        this.sm = sm;
        sm.SetupPatrol(this);
        DecideCurrentTarget();
        km = sm.gameObject.GetComponent<KinematicController>();
        km.SetTarget(currentTarget);
        EnemyAnimationController animationController = sm.GetAnimator();
        animationController.SetParameter(StateTypes.Patrolling, true);
    }

    private void SetCurrentTarget(Vector3 target)
    {
        currentTarget = target;
    }

    private void DecideCurrentTarget()
    {
        Vector3 currentPosition = sm.gameObject.transform.position;
        float distanceTarget1 = Vector3.Distance(currentPosition, point1);
        float distanceTarget2 = Vector3.Distance(currentPosition, point2);
        
        if (distanceTarget1 <= distanceTarget2)
        {
            SetCurrentTarget(point2);
        } else
        {
            SetCurrentTarget(point1);
        }
    }

}

public class AttackingState : State
{
    StateMachine sm;
    public Vector3 target;
    public float tolerance;
    KinematicController km;

    protected override void OnStateEnd(StateTypes nextState)
    {
        km.ResetSpeed();
        km.ResetCleanup();
        EnemyAnimationController animationController = sm.GetAnimator();
        animationController.SetParameter(StateTypes.Attacking, false);
        sm.SetState(nextState);
    }

    protected override void OnStateUpdate()
    {
        // Move towards recorder player position
        km.MoveTowardsTarget();
        // If it has reached the player position, move to a recovery time
        if (km.DestinationWithinTolerance(sm.gameObject.transform.position, tolerance))
        {
            EndState(StateTypes.Recovering);
        }
    }

    protected override void OnStateStart(StateMachine sm)
    {
        this.sm = sm;
        km = sm.gameObject.GetComponent<KinematicController>();
        EnemyAnimationController animationController = sm.GetAnimator();
        animationController.SetParameter(StateTypes.Attacking, true);
        sm.SetupAttack(this);
        km.SetTarget(target);
    }
}

public class RecoveringState : State
{
    StateMachine sm;
    KinematicController km;
    public float idleTime;
    float internalTimer;
    protected override void OnStateEnd(StateTypes nextState)
    {
        km.ResetCleanup();
        EnemyAnimationController animationController = sm.GetAnimator();
        animationController.SetParameter(StateTypes.Recovering, false);
        sm.SetState(nextState);
    }

    protected override void OnStateUpdate()
    {
        internalTimer += Time.deltaTime;
        km.ResetHeight();
        if (internalTimer >= idleTime)
        {
            EndState(StateTypes.Patrolling);
        }
    }

    protected override void OnStateStart(StateMachine sm)
    {

        this.sm = sm;
        km = sm.GetComponent<KinematicController>();
        km.SetStateChangePosition();
        EnemyAnimationController animationController = sm.GetAnimator();
        animationController.SetParameter(StateTypes.Recovering, true);
        sm.SetupRecovery(this);
    }
}

public class AlertedState : State
{
    public Vector3 target;
    public StateMachine sm;
    float idleTimer = 0.0f;
    protected override void OnStateEnd(StateTypes nextState)
    {
        sm.SetState(nextState);
    }

    protected override void OnStateStart(StateMachine sm)
    {
        this.sm = sm;
        sm.SetupAlert(this);
        KinematicController km = sm.gameObject.GetComponent<KinematicController>();
        EnemyAnimationController em = sm.gameObject.GetComponent<EnemyAnimationController>();
        em.ShowAlert();
        km.SetRushSpeed();
        km.SetTarget(target);
    }

    protected override void OnStateUpdate()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer > 1.0f)
        {
            EndState(StateTypes.Attacking);
        }
    }

}

public static class StateFactory
{
    public static State MakeState(State.StateTypes type)
    {
        switch (type)
        {
            case State.StateTypes.Patrolling:
                return new PatrollingState();
            case State.StateTypes.Attacking:
                return new AttackingState();
            case State.StateTypes.Alerted: 
                return new AlertedState();
            case State.StateTypes.Recovering:
                return new RecoveringState();
            default:
                return new IdleState();
        }
    }
}