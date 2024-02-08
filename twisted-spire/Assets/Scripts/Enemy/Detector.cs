using UnityEngine;

public class Detector : MonoBehaviour
{
    public float radius;
    SphereCollider sphereCollider;
    EnemyPatrolBase ebase;
    
    public void Setup(EnemyPatrolBase eBase, Vector3 colliderCenter)
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radius;
        sphereCollider.center = colliderCenter;
        ebase = eBase;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player is in detection radius");
            ebase.ReactToDetection(other.gameObject.transform);
        }
    }

}
