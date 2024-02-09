using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine : MonoBehaviour
{
    public State.StateTypes startingState;
    public State.StateTypes currentStateType;
    State currentState;
    Vector3 playerPosition;
    public bool canAttack;
    private bool playerInRange;
    public float patrolTheta;
    public float distanceTolerance;
    public float recoveryTime;
    public float idleTime;

    // Start is called before the first frame update
    void Start()
    {
        currentStateType = startingState;
        currentState = StateFactory.MakeState(currentStateType);
        currentState.StartState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    public void SetState(State.StateTypes state)
    {
        currentStateType = state;
        currentState = StateFactory.MakeState(currentStateType);
        currentState.StartState(this);
    }

    public void SetupPatrol(PatrollingState state)
    {
        KinematicController km = GetComponent<KinematicController>();
        state.point1 = UtilityFunctions.RotateClockwise(km.initialPosition, patrolTheta);
        state.point2 = UtilityFunctions.RotateCounterClockwise(km.initialPosition, patrolTheta);
        state.tolerance = distanceTolerance;
    }

    public void SetupAttack(AttackingState state)
    {
        state.tolerance = distanceTolerance;
        state.target = playerPosition;
    }

    public void SetupRecovery(RecoveringState state)
    {
        state.idleTime = recoveryTime;
    }

    public void SetupIdle(IdleState state)
    {
        
        state.idleTime = idleTime;
    }

    public void SetupAlert(AlertedState state)
    {
        state.target = playerPosition;
    }

    public bool ShouldAttack()
    {
        return canAttack && playerInRange;
    }

    public void TargetPlayer(Vector3 playerPosition)
    {
        this.playerPosition = playerPosition;
        playerInRange = true;

    }

    public void LoseTarget()
    {
        playerInRange = false;
    }
}
