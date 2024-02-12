using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballHandler : MonoBehaviour
{
    public float speed = 0f;
    public float lifetime = 3f;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;

        lifetime -= Time.deltaTime;
        if (lifetime < 0f)
        {
            Destroy(gameObject, 1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController pc))
        {
            pc.ResetToSpawner();
        }

        Destroy(gameObject, 1f); // delay to allow any explosion effects to play first
    }
}
