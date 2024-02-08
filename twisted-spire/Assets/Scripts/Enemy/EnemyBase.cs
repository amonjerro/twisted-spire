using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Tooltip("The radius from the center of the level. The player moves along a horizonal circumference of this radius.")]
    public float levelRadius = 10f;
    public float spriteHeight;
    SpriteRenderer childSM;
    BoxCollider col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        col.center = transform.right * levelRadius;
        transform.localPosition += col.center;
        col.center = new Vector3(0, spriteHeight, 0);
        childSM = transform.GetChild(0).GetComponent<SpriteRenderer>();
        KinematicController km = GetComponent<KinematicController>();
        km.initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFlipSprite(bool state)
    {
        childSM.flipX = state;
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
