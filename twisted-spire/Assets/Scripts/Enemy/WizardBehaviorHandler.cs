using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBehaviorHandler : Enemy
{
    public GameObject fireball;
    public float fireballSpeed = 5f;
    public float fireballDamage = 33f;

    public float laserDuration = 5f;
    public float laserCount = 4f;
    public float laserRotationSpeedDeg = 10f;

    public float phase2Threshold = 0.67f;
    public float phase3Threshold = 0.33f;

    bool aggro = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (!fireball)
        {
            Debug.LogError("ERROR: Wizard does not have a Fireball prefab set!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastFireball(Vector3 direction, float speed, float damage)
    {

    }

    /// <summary>
    /// Sets if the wizard should start trying to kill the player.
    /// </summary>
    /// <param name="aggro">If the wizard is trynig to kill the player or not.</param>
    public void SetAggro(bool aggro)
    {
        this.aggro = aggro;
    }

    public override bool Immune { get => base.Immune; set => base.Immune = value; }
}
