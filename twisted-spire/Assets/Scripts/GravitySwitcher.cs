using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitcher : MonoBehaviour
{
    public bool flipParticles = false;
    BoxCollider col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        if (flipParticles)
        {
            GetComponentInChildren<ParticleSystem>().transform.rotation = Quaternion.Euler(90, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController pc))
        {
            Physics.gravity *= -1f;
            pc.FlipSprite(Physics.gravity.y >= 0f);
        }
    }
}
