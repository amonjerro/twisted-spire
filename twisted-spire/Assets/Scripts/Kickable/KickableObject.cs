using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KickableObject : MonoBehaviour
{
    public GameObject target;
    private float _distance;
    private Vector3 _direction;
    Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        CalculateKick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Kick()
    {
        rb.AddForce(_direction * _distance * 0.5f, ForceMode.Impulse);
    }

    private void CalculateKick()
    {
        _distance = Vector3.Distance(transform.position, target.gameObject.transform.position);
        _direction = target.gameObject.transform.position - transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                // Make sure the player goes into Kick animation

                // Kick this thing
                Kick();
                break;
        }

        // This should be handled uniquely by any class that overrides the KickableTarget abstract class
        if (collision.gameObject.TryGetComponent(out IKickableTarget kick))
        {
            kick.OnKicked();
        }
    }
}
