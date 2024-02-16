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
    public float moveDrag = -50f;

    [Tooltip("Player's airborne control.")]
    public float airborneControl = 0.1f;

    [Tooltip("Player's jump height.")]
    public float jumpHeight = 3f;

    [Tooltip("The max incline slope the player can traverse.")]
    public float maxIncline = 45f;

    public GameObject modelPivot;

    [Tooltip("The Spawner object to return to when its time to restart. Will be useful for checkpoints")]
    public GameObject Spawner;

    public bool isDead = false;

    bool airborne = false;
    Vector3 groundNormal;
    Rigidbody rb;
    SpriteRenderer sp;
    Animator pc_animator;
    CapsuleCollider col;

    float jumpCD = 0.5f;
    float jumpTmr = 0f;
    bool jumpReady = false;

    // Start is called before the first frame update
    void Start()
    {
        sp = modelPivot.GetComponentInChildren<SpriteRenderer>();
        pc_animator = modelPivot.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        UpdateSpawnerLocation(transform.position);
        col.center = transform.right * levelRadius;
        modelPivot.transform.localPosition = new Vector3(col.center.x, col.center.y);
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
            sp.flipX = false;
            vel.x = 1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
        {
            sp.flipX = true;
            vel.x = -1f;
        }

        CheckOnRamp(vel);

        float airCtrl = airborne ? airborneControl : 1f;
        pc_animator.SetFloat("f_movespeed", Mathf.Abs(vel.x));
        Vector3 newDir = transform.forward * vel.x;
        float dot = Vector3.Dot(groundNormal, newDir);
        newDir *= (1f - dot); // newDir.y should always be 0
        vel = new Vector2(newDir.magnitude * vel.x * airCtrl, -dot * moveAccel * Time.deltaTime);
        
        // add drag
        vel.x += -rb.angularVelocity.y * moveDrag * airCtrl;

        // Jumping
        jumpReady = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (jumpReady && !airborne)
        {
            vel.y = jumpHeight * -Physics.gravity.normalized.y;
            airborne = true;
            jumpTmr = jumpCD;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
        }
        rb.AddForce(new Vector3(0f, vel.y, 0f));
        rb.AddTorque(0f, vel.x * moveAccel * Time.deltaTime, 0f, ForceMode.Acceleration);
    }

    void CheckOnRamp(Vector2 vel)
    {
        groundNormal = Vector3.zero;
        Vector3 grav = Physics.gravity.normalized;
        if (Physics.SphereCast(transform.position + (transform.right * levelRadius), col.radius * 0.95f, grav, out RaycastHit hit, (col.height / 2f) - col.radius + 0.1f, 1))
        {
            // if angle between ground normal and player's up axis
            // is >= 15, add ramp assist
            if (Math.Acos(Vector3.Dot(hit.normal, -grav)) / Mathf.Deg2Rad > 15)
            {
                groundNormal = hit.normal;
                rb.AddTorque(0f, vel.x * moveAccel * Time.deltaTime / 2, 0f, ForceMode.Acceleration);
            }
        }
    }

    public Vector3 GetColliderPosition()
    {
        return transform.TransformPoint(transform.right * levelRadius);
    }

    void CheckAirborne()
    {
        groundNormal = Vector3.zero;
        bool nowAirborne = true;
        Vector3 grav = Physics.gravity.normalized;
        if (Physics.SphereCast(transform.position + (transform.right * levelRadius), col.radius * 0.95f, grav, out RaycastHit hit, (col.height / 2f) - col.radius + 0.1f, 1))
        {
            // if angle between ground normal and player's up axis
            // is <= the max incline, it is a valid ground
            if (Math.Acos(Vector3.Dot(hit.normal, -grav)) <= maxIncline * Mathf.Deg2Rad)
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
        FallingChandelier[] chandeliers = FindObjectsOfType<FallingChandelier>();
        Debug.Log(chandeliers.Length);
        foreach (var chandelier in chandeliers)
        {
            Debug.Log("reset");
            chandelier.ResetChandelier(); // Call the reset method on all chandeliers
        }
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

    public void FlipSprite(bool upsideDown)
    {
        sp.transform.localPosition = new Vector3(0f, upsideDown ? 1f : -1f, 0f);
        sp.flipY = upsideDown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out KickableObject kick))
        {
            Debug.Log(other.gameObject.name);
            // Player kick animation here
            kick.Kick();
        }
    }
}
