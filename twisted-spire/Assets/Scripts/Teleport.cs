using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public float teleportDistance = 10f; // Distance to teleport the player upwards

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 newPosition = other.transform.position + Vector3.up * teleportDistance; // Calculate the new position
            other.transform.position = newPosition; // Teleport the player to the new position
        }
    }
}
