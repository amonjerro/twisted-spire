using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KinematicController : MonoBehaviour
{
    
    public float rotateSpeed;
    private float _initialRotateSpeed;
    bool rotateCounterClockWise;
    EnemyPatrolBase baseController;
    public Vector3 initialPosition;
    private Vector3 targetPosition;
    private Vector3 stateChangePosition;
    public bool allowVerticalMovement;
    private float _resetTime;

    private void Start()
    {
        baseController = GetComponent<EnemyPatrolBase>();
        _initialRotateSpeed = rotateSpeed;
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;

        // Calculate rotation direction
        target.y = transform.position.y;
        float dot = Vector3.Dot(transform.position.normalized, target.normalized);
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;
        Vector3 ccwPosition = UtilityFunctions.RotateCounterClockwise(transform.position, theta);
        Vector3 cwPosition = UtilityFunctions.RotateClockwise(transform.position, theta);
        float ccwDistance = Vector3.Distance(ccwPosition, target);
        float cwDistance = Vector3.Distance(cwPosition, target);
        rotateCounterClockWise = ccwDistance < cwDistance;

        if (rotateCounterClockWise)
        {
            baseController.SetFlipSprite(true);
        } else
        {
            baseController.SetFlipSprite(false);
        }

        // If I can't move vertically, I haven't got anything else to do
        if (!allowVerticalMovement)
        {
            return;
        }
        // Otherwise
        // Calculate height delta
        target = targetPosition;
        stateChangePosition = transform.position;

    }

    public void SetStateChangePosition()
    {
        stateChangePosition = transform.position;
    }

    public void MoveTowardsTarget()
    {
        RotateTowardsTarget();
        if (allowVerticalMovement)
        {
            ElevateTowardsTarget();
        }
        
    }

    public bool DestinationWithinTolerance(Vector3 position, float tolerance)
    {

       Vector3 heightAgnosticPosition = new Vector3(targetPosition.x, position.y, targetPosition.z);
       return Vector3.Distance(position, heightAgnosticPosition) <= tolerance;

    }

    public void SetRushSpeed()
    {
        rotateSpeed = _initialRotateSpeed * 1.5f;
    }

    private void RotateTowardsTarget()
    {
        float theta = Time.deltaTime * rotateSpeed;
        if (rotateCounterClockWise)
        {
            transform.position = UtilityFunctions.RotateCounterClockwise(transform.position, theta);
        }
        else
        {
            transform.position = UtilityFunctions.RotateClockwise(transform.position, theta);
        }
    }

    public void ResetSpeed()
    {
        rotateSpeed = _initialRotateSpeed;
    }


    public void ElevateTowardsTarget()
    {
        if (!allowVerticalMovement)
        {
            return;
        }
        _resetTime = Mathf.Clamp01(_resetTime + Time.deltaTime);
        float newYposition = UtilityFunctions.Lerp(stateChangePosition.y, targetPosition.y, _resetTime);
        transform.position = new Vector3(transform.position.x, newYposition, transform.position.z);
    }

    public void ResetHeight()
    {
        if (!allowVerticalMovement)
        {
            return;
        }
        _resetTime = Mathf.Clamp01(_resetTime + Time.deltaTime);
        float newYposition = UtilityFunctions.Lerp(stateChangePosition.y, initialPosition.y, _resetTime);
        transform.position = new Vector3(transform.position.x, newYposition, transform.position.z);
    }

    public void ResetCleanup()
    {
        _resetTime = 0f;
    }
}
