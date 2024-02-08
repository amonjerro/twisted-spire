using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public abstract class State { 
    public enum StateTypes
    {
        Idle,
        Patrolling,
        Alerted,
        Attacking,
        Recovering
    }

    public abstract void OnStateEnd(StateTypes nextState);
    public abstract void OnStateStart(StateMachine sm);
    public abstract void OnStateUpdate();

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
    bool isActive = true;
    public float idleTime;
    float internalTimer;
    public override void OnStateEnd(StateTypes nextState)
    {
        sm.SetState(nextState);
    }

    public override void OnStateStart(StateMachine sm)
    {
        this.sm = sm;
        sm.SetupIdle(this);
        internalTimer = 0.0f;
    }

    public override void OnStateUpdate()
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

    public override void OnStateEnd(StateTypes nextState)
    {
        sm.SetState(nextState);
    }

    public override void OnStateUpdate()
    {
        // Test for attack state
        if (sm.ShouldAttack())
        {
            EndState(StateTypes.Alerted);
        }

        // Move towards the current target
        km.MoveTowardsTarget();
        if (Vector3.Distance(sm.gameObject.transform.position, currentTarget) <= tolerance)
        {
            EndState(StateTypes.Idle);
        }
    }

    public override void OnStateStart(StateMachine sm)
    {
        this.sm = sm;
        sm.SetupPatrol(this);
        DecideCurrentTarget();
        km = sm.gameObject.GetComponent<KinematicController>();
        km.SetTarget(currentTarget);
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

    public override void OnStateEnd(StateTypes nextState)
    {
        km.ResetSpeed();
        sm.SetState(nextState);
    }

    public override void OnStateUpdate()
    {
        // Move towards recorder player position
        km.MoveTowardsTarget();

        // If it has reached the player position, move to a recovery time
        if (Vector3.Distance(sm.gameObject.transform.position, target) < tolerance)
        {
            EndState(StateTypes.Recovering);
        }
    }

    public override void OnStateStart(StateMachine sm)
    {
        this.sm = sm;
        km = sm.gameObject.GetComponent<KinematicController>();
        sm.SetupAttack(this);
        km.SetTarget(target);
    }
}

public class RecoveringState : State
{
    StateMachine sm;
    public float idleTime;
    float internalTimer;
    public override void OnStateEnd(StateTypes nextState)
    {
        sm.SetState(nextState);
    }

    public override void OnStateUpdate()
    {
        internalTimer += Time.deltaTime;
        if (internalTimer >= idleTime)
        {
            EndState(StateTypes.Patrolling);
        }
    }

    public override void OnStateStart(StateMachine sm)
    {
        this.sm = sm;
        sm.SetupRecovery(this);
    }
}

public class AlertedState : State
{
    public Vector3 target;
    public StateMachine sm;
    float idleTimer = 0.0f;
    public override void OnStateEnd(StateTypes nextState)
    {
        sm.SetState(nextState);
    }

    public override void OnStateStart(StateMachine sm)
    {
        this.sm = sm;
        sm.SetupAlert(this);
        KinematicController km = sm.gameObject.GetComponent<KinematicController>();
        km.SetRushSpeed();
        km.SetTarget(target);
    }

    public override void OnStateUpdate()
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