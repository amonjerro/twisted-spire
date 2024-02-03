using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Tooltip("The radius from the center of the level. The player moves along a horizonal circumference of this radius.")]
    public float levelRadius = 10f;

    [Tooltip("Player's movement acceleration.")]
    public float moveAccel = 100f;

    [Tooltip("Player's movement drag.")]
    public float moveDrag = -10f;

    [Tooltip("Player's airborne control.")]
    public float airborneControl = 0.1f;

    [Tooltip("Player's jump height.")]
    public float jumpHeight = 3f;

    [Tooltip("The max incline slope the player can traverse.")]
    public float maxIncline = 45f;

    public GameObject model;

    [Tooltip("The Spawner object to return to when its time to restart. Will be useful for checkpoints")]
    public GameObject Spawner;

    public bool isDead = false;

    bool airborne = false;
    Vector3 groundNormal;
    Rigidbody rb;
    CapsuleCollider col;

    float jumpCD = 0.25f;
    float jumpTmr = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        UpdateSpawnerLocation(transform.position);
        col.center = transform.right * levelRadius;
        model.transform.localPosition = col.center;
    }

    // Update is called once per frame
    void Update()
    {
        jumpTmr -= Time.deltaTime;

        // Update the player's airborne status
        if (jumpTmr <= 0f)
            CheckAirborne();

        // All movement is actually just angular velocity on a capsule collider
        // offset by levelRadius from the local origin.
        // vel.x is the horizontal angular velocity
        // vel.y is the vertical linear velocity
        Vector2 vel = Vector2.zero;

        // horizontal movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            vel.x = 1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
        {
            vel.x = -1f;
        }

        float airCtrl = airborne ? airborneControl : 1f;

        Vector3 newDir = transform.forward * vel.x;
        float dot = Vector3.Dot(groundNormal, newDir);
        newDir *= (1f - dot); // newDir.y should always be 0
        vel = new Vector2(newDir.magnitude * vel.x * airCtrl, -dot * moveAccel * Time.deltaTime);

        // add drag
        vel.x += -rb.angularVelocity.y * moveDrag * airCtrl;

        if (!airborne)
        {
            // Jumping
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                vel.y = jumpHeight;
                airborne = true;
                jumpTmr = jumpCD;
                rb.useGravity = true;
                rb.velocity = Vector3.zero;
            }
        }
        rb.AddForce(new Vector3(0f, vel.y, 0f));
        rb.AddTorque(0f, vel.x * moveAccel * Time.deltaTime, 0f, ForceMode.Acceleration);
    }

    void CheckAirborne()
    {
        groundNormal = Vector3.zero;
        bool nowAirborne = true;

        if (Physics.SphereCast(transform.position + (transform.right * levelRadius), col.radius * 0.95f, -Vector3.up, out RaycastHit hit, (col.height / 2f) - col.radius + 0.03f, 1))
        {
            // if angle between ground normal and player's up axis
            // is <= the max incline, it is a valid ground
            if (Math.Acos(Vector3.Dot(hit.normal, Vector3.up)) <= maxIncline * Mathf.Deg2Rad)
            {
                groundNormal = hit.normal;
                nowAirborne = false;
            }
        }
        airborne = nowAirborne;
        rb.useGravity = airborne;
    }

    public void ResetToSpawner()
    {
        transform.position = Spawner.transform.position;
        isDead = true;
    }
    
    // When the player passes a checkpoint, call this function. Sets the spawner
    // to the checkpoint's Y position and its rotation to the player's rotation
    // so that the spawn correctly puts the player at the right height.
    public void UpdateSpawnerLocation(Vector3 position)
    {
        Spawner.transform.position = position;
        Spawner.transform.rotation = transform.rotation;
    }
}
