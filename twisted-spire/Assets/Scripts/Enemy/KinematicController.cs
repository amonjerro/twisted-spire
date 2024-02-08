using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KinematicController : MonoBehaviour
{
    public float rotateSpeed;
    private float _initialRotateSpeed;
    bool rotateCounterClockWise;
    EnemyPatrolBase baseController;
    public Vector3 initialPosition;

    private void Start()
    {
        baseController = GetComponent<EnemyPatrolBase>();
        _initialRotateSpeed = rotateSpeed;
    }

    public void SetTarget(Vector3 target)
    {
        float dot = Vector3.Dot(transform.position.normalized, target.normalized);
        float theta = Mathf.Acos(dot) * Mathf.Deg2Rad;
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
        float theta = Time.deltaTime * rotateSpeed;
        if (rotateCounterClockWise)
        {
            transform.position = UtilityFunctions.RotateCounterClockwise(transform.position, theta);
        } else
        {
            transform.position = UtilityFunctions.RotateClockwise(transform.position, theta);
        }
    }

    public void SetRushSpeed()
    {
        rotateSpeed = _initialRotateSpeed * 1.5f;
    }

    public void ResetSpeed()
    {
        rotateSpeed = _initialRotateSpeed;
    }

    public void ResetHeight()
    {

    }
}
