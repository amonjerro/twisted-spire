using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KickableObject : MonoBehaviour
{
    public GameObject target;
    private float _distance;
    private Vector3 _direction;
    protected Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Kick()
    {
        // exclude player layer from all colliders n this object
        // so it can't be kicked multiple times
        foreach (Collider c in GetComponents<Collider>())
        {
            c.excludeLayers |= (1 << 3);
        }
        
        rb.isKinematic = false;
        if (target)
        {
            CalculateKick();
            rb.AddForce(_direction * _distance, ForceMode.VelocityChange);
        }
    }

    private void CalculateKick()
    {
        _distance = Vector3.Distance(transform.position, target.transform.position);
        _direction = (target.transform.position - transform.position).normalized;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ayo");
        // This should be handled uniquely by any class that overrides the KickableTarget abstract class
        if (collision.gameObject.TryGetComponent(out IKickableTarget kick))
        {
            kick.OnKicked();
            Destroy(gameObject);
        }
    }
}
