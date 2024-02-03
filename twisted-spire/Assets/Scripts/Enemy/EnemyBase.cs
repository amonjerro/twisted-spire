using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Tooltip("The radius from the center of the level. The player moves along a horizonal circumference of this radius.")]
    public float levelRadius = 10f;

    BoxCollider col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        col.center = transform.right * levelRadius;
        transform.localPosition += col.center;
        col.center = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.ResetToSpawner();
        }
        
    }
}
