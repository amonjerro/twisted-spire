using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KickableObject : MonoBehaviour
{
    public KickableTarget target;
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
            case "KickTarget":
                target.ActivateDependent();
                Destroy(gameObject);
                break;
            case "Wizard":
                // Deal damage to the wizard?
                Destroy(gameObject);
                break;
        }

    }
}
