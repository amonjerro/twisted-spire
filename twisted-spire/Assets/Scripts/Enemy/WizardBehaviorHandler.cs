using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardBehaviorHandler : Enemy, IKickableTarget
{
    EnemyAnimationController eac;
    public GameObject fireball;
    public GameObject healthBar;
    
    [Tooltip("Speed fireballs will be travelling at")]
    public float fireballSpeed = 10f;

    [Tooltip("Telegraphing time")]
    public float fireballStartCastAnim = .5f;

    [Tooltip("Minimum cooldown time between shots")]
    public float fireballMinCD = 3f;

    [Tooltip("Maximum cooldown time between shots")]
    public float fireballMaxCD = 6f;

    // Not implemented
    public float laserDuration = 5f;
    public float laserCount = 4f;
    public float laserRotationSpeedDeg = 10f;
    // Not implemented
    public float phase2Threshold = 0.67f;
    public float phase3Threshold = 0.33f;

    [Tooltip("Is the wizard ready to start fighting")]
    public bool aggro = false;

    [Tooltip("Maximum miss offset, in degrees")]
    public float aimNoise = 0.0f;
    private bool isCasting = false;
    float fireTmr = 0f;

    // player component references
    Rigidbody rb;
    PlayerController pc;

    // Start is called before the first frame update
    protected override void Start()
    {
        eac = GetComponent<EnemyAnimationController>();
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

        if (aggro && fireTmr <= 0f)
        {
            fireTmr = Random.Range(fireballMinCD, fireballMaxCD);
            float noiseTheta = Random.Range(0.0f, aimNoise);
            isCasting = false;
            // Calculate the trajectory of the fireball's path based on the player's velocity
            eac.WizardTrigger("Attack");
            Vector3 playerVel = Vector3.Cross(toPlayer, Vector3.up).normalized * (rb.angularVelocity.y * pc.levelRadius) - rb.velocity;

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
                orientation = Quaternion.AngleAxis(theta+noiseTheta, Vector3.Cross(playerVel, toPlayer).normalized) * orientation;

                CastFireball(orientation, fireballSpeed);
            }
            
        }else if (aggro && fireTmr <= fireballStartCastAnim && !isCasting)
        {
            isCasting = true;
            eac.WizardTrigger("StartCasting");
        }
    }

    public void CastFireball(Quaternion orientation, float speed)
    {
        GameObject newFireball = Instantiate(fireball, transform.position + transform.forward, orientation);
        newFireball.GetComponent<FireballHandler>().speed = speed;
        newFireball.GetComponent<FireballHandler>().target = gameObject;
    }

    /// <summary>
    /// Sets if the wizard should start trying to kill the player.
    /// </summary>
    /// <param name="aggro">If the wizard is trynig to kill the player or not.</param>
    public void SetAggro(bool aggro)
    {
        this.aggro = aggro;
        healthBar.SetActive(true);
        fireTmr = Random.Range(fireballMinCD, fireballMaxCD);
    }

    public void OnKicked()
    {
        TakeDamage(20f);
        healthBar.GetComponent<Slider>().value -= 20;

        
    }
}
