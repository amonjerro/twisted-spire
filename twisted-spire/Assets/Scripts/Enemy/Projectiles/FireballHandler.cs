using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class FireballHandler : KickableObject
{
    public float speed = 0f;
    public float lifetime = 5f;
    float lifeTmr = 0f;
    bool idle = false;

    // Start is called before the first frame update
    void Start()
    {
        lifeTmr = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!idle)
        {
            rb.velocity = transform.forward * speed;
        }

        lifeTmr -= Time.deltaTime;
        if (lifeTmr < 0f)
        {
            Destroy(gameObject);
        }
    }

    public override void Kick()
    {
        // refresh lifetime
        lifeTmr = lifetime;
        base.Kick();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController pc))
        {
            pc.ResetToSpawner();
            Destroy(gameObject);
        }
        else
        {
            rb.isKinematic = true;
            idle = true;
            // no longer exclude player so player can lob this fireball back at the wizard
            GetComponents<SphereCollider>()[0].excludeLayers |= (1 << 3); // actual collider
            GetComponents<SphereCollider>()[1].excludeLayers &= ~(1 << 3); // trigger collider
        }
        base.OnCollisionEnter(collision);
    }
}
