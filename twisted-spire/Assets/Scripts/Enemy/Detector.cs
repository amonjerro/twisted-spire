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
        if (other.tag == "Player")
        {
            Transform modelPivot = other.transform.GetChild(0);
            ebase.ReactToDetection(modelPivot.transform.position);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        ebase.LoseTarget();
    }
}
