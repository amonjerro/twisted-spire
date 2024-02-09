using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class KinematicController : MonoBehaviour
{
    
    public float rotateSpeed;
    private float _initialRotateSpeed;
    bool rotateCounterClockWise;
    EnemyPatrolBase baseController;
    public Vector3 initialPosition;
    private Vector3 targetPosition;
    public bool allowVerticalMovement;

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

    }

    public void MoveTowardsTarget()
    {
        RotateTowardsTarget();
        if (allowVerticalMovement)
        {
            
        }
        
    }

    public bool DestinationWithinTolerance(Vector3 position, float tolerance)
    {
        if (allowVerticalMovement)
        {
            return Vector3.Distance(position, targetPosition) <= tolerance;
        } else
        {
            Vector3 heightAgnosticPosition = new Vector3(targetPosition.x, position.y, targetPosition.z);
            return Vector3.Distance(position, heightAgnosticPosition) <= tolerance;
        }
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


    // To do
    public void ElevateTowardsTarget()
    {

    }

    public void ResetHeight()
    {

    }
}
