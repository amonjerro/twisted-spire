using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class FireballHandler : MonoBehaviour
{
    public float speed = 0f;
    public float lifetime = 5f;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!rb.isKinematic)
        {
            rb.velocity = transform.forward * speed;
        }

        lifetime -= Time.deltaTime;
        if (lifetime < 0f)
        {
            DestroyFireball();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController pc))
        {
            pc.ResetToSpawner();
            DestroyFireball();
            
        }
        else if (collision.gameObject.TryGetComponent(out WizardBehaviorHandler wiz))
        {
            wiz.TakeDamage(10f);
            DestroyFireball();
        }
        else
        {
            rb.isKinematic = true;
            // no longer exclude player so player can lob this fireball back at the wizard
            GetComponents<SphereCollider>()[0].excludeLayers |= (1 << 3);
            GetComponents<SphereCollider>()[1].excludeLayers &= ~(1 << 3);
        }
    }

    private void DestroyFireball()
    {
        enabled = false;

        Destroy(rb);

        foreach (SphereCollider col in GetComponents<SphereCollider>())
            Destroy(col);

        Destroy(gameObject, 1f); // delay to allow any explosion effects to play first
    }
}
