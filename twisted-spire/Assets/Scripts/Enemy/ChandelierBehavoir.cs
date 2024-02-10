using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;

public class FallingChandelier : MonoBehaviour
{
    //public float detectionColliderHeight = 1.095433f; // Height of the detection collider
    //public float detectionColliderZPosition = -1.14f; // Z-axis position of the detection collider
    private bool hasFallen = false; // To ensure it only falls once
    private bool hitGround = false; // Remove hit collision once ground is hit

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Rigidbody rb; // Reference to the Rigidbody component
    private CapsuleCollider detectionCollider; // Reference to the already attached detection collider
    void Awake()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        // Get the reference to the already attached CapsuleCollider
        detectionCollider = GetComponent<CapsuleCollider>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Start()
    { 
      
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the chandelier has not fallen yet and if the other collider has the tag "Player"
        if (!hasFallen && other.CompareTag("Player"))
        {
            System.Random r = new System.Random();
            float randoTime = (float)(r.NextDouble()*0.3);
            Invoke("Fall", randoTime);
            Debug.Log("Delay time: " + randoTime);
        }
       
    }

    void Fall()
    {
        // Make the Rigidbody dynamic to let it fall
        rb.isKinematic = false;
        hasFallen = true;
    }

    async void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Detected with " + collision.gameObject.name);
        // Check if the chandelier collides with the player upon falling
        if (collision.gameObject.CompareTag("Player") && hasFallen && !hitGround)
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.ResetToSpawner();
            //ResetChandelier();
            DestroyAndRespawn();
        }
        else if (collision.gameObject.CompareTag("Floor") && hasFallen)
        {
            Debug.Log("Hit Ground");
            hitGround = true;
            await Task.Delay(500);
            DestroyAndRespawn();
        }
    }

    void ResetChandelier()
    {
        rb.isKinematic = true;
        hasFallen = false;
        hitGround = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    async void DestroyAndRespawn()
    {
        gameObject.SetActive(false); // Hide the chandelier;
        //Invoke("ResetChandelier", 1.5f);
        //gameObject.SetActive(true); // Show the chandelier again
    }
    /*
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, detectionColliderHeight / 2, detectionColliderZPosition), 0.5f);
    }
    */
}
