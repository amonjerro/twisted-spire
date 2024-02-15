using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBehaviorHandler : Enemy, IKickableTarget
{
    public GameObject fireball;
    public float fireballSpeed = 10f;
    public float fireballMinCD = 3f;
    public float fireballMaxCD = 6f;

    public float laserDuration = 5f;
    public float laserCount = 4f;
    public float laserRotationSpeedDeg = 10f;

    public float phase2Threshold = 0.67f;
    public float phase3Threshold = 0.33f;

    bool aggro = false;
    float fireTmr = 0f;

    // player component references
    Rigidbody rb;
    PlayerController pc;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (!fireball)
        {
            Debug.LogError("ERROR: Wizard does not have a Fireball prefab set!");
        }

        rb = player.GetComponent<Rigidbody>();
        pc = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        fireTmr -= Time.deltaTime;

        // orient wizard to always look towards the player
        Vector3 toPlayer = player.transform.GetChild(0).position - transform.position;
        Quaternion orientation = Quaternion.LookRotation(toPlayer);
        transform.rotation = orientation;

        if (fireTmr <= 0f)
        {
            fireTmr = Random.Range(fireballMinCD, fireballMaxCD);

            // Calculate the trajectory of the fireball's path based on the player's velocity
            
            Vector3 playerVel = Vector3.Cross(toPlayer, Vector3.up).normalized * (rb.angularVelocity.y * pc.levelRadius) + rb.velocity;

            // Just cast a firefball directly at the player if they're not moving
            if (playerVel == Vector3.zero)
            {
                CastFireball(orientation, fireballSpeed);
            }
            // Alter the fireball's trajectory based on the player's movement
            else
            {
                float phi = Vector3.Angle(-toPlayer, playerVel) * Mathf.Deg2Rad;
                
                float theta = Mathf.Asin(playerVel.magnitude * Mathf.Sin(phi) / fireballSpeed) * Mathf.Rad2Deg;
                orientation = Quaternion.AngleAxis(theta, Vector3.Cross(playerVel, toPlayer).normalized) * orientation;

                CastFireball(orientation, fireballSpeed);
            }
        }
    }

    public void CastFireball(Quaternion orientation, float speed)
    {
        GameObject newFireball = Instantiate(fireball, transform.position + (transform.forward), orientation);
        newFireball.GetComponent<FireballHandler>().speed = speed;
    }

    /// <summary>
    /// Sets if the wizard should start trying to kill the player.
    /// </summary>
    /// <param name="aggro">If the wizard is trynig to kill the player or not.</param>
    public void SetAggro(bool aggro)
    {
        this.aggro = aggro;
    }

    public void OnKicked()
    {

    }

    public override bool Immune { get => base.Immune; set => base.Immune = value; }
}
