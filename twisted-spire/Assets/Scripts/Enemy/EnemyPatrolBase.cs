using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolBase : Enemy
{
    [Tooltip("The radius from the center of the level. The player moves along a horizonal circumference of this radius.")]
    public float levelRadius = 10f;
    public float spriteHeight;
    SpriteRenderer childSM;
    BoxCollider col;
    StateMachine stateMachine;
    Detector detector;
    // Start is called before the first frame update
    protected override void Start()
    {
        // do generic Enemy setup first
        base.Start();

        col = GetComponent<BoxCollider>();
        stateMachine = GetComponent<StateMachine>();
        col.center = transform.right * levelRadius;
        transform.localPosition += col.center;
        col.center = new Vector3(0, spriteHeight, 0);
        childSM = transform.GetChild(0).GetComponent<SpriteRenderer>();
        detector = transform.GetChild(1).GetComponent<Detector>();
        detector.Setup(this, col.center);
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

    public void ReactToDetection(Vector3 playerPosition)
    {
        stateMachine.TargetPlayer(playerPosition);
    }

    public void LoseTarget()
    {
        stateMachine.LoseTarget();
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
