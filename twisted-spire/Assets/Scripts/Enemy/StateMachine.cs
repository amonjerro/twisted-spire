using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine : MonoBehaviour
{
    [Tooltip("The starting state for this enemy")]
    public State.StateTypes startingState;

    [Tooltip("A debug look at the current state of the enemy")]
    public State.StateTypes currentStateType;
    State currentState;
    Vector3 playerPosition;

    [Tooltip("Control for whether this enemy can attack or will just patrol")]
    public bool canAttack;

    [Tooltip("The patrol radius in degrees. Bigger means a longer patrol")]
    public float patrolTheta;

    [Tooltip("Tolerance for determining that this enemy has reached a destination")]
    public float distanceTolerance;
    
    [Tooltip("The amount of time this enemy spends recovering after attacking")]
    public float recoveryTime;

    [Tooltip("The amount of time this enemy spends idle between patrol points")]
    public float idleTime;

    private bool playerInRange;
    private EnemyAnimationController animationController;
    

    // Start is called before the first frame update
    void Start()
    {
        animationController = GetComponent<EnemyAnimationController>();
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

    public EnemyAnimationController GetAnimator()
    {
        return animationController;
    }
}
