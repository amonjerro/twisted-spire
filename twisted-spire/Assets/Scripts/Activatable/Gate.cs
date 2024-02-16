using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IActivatable
{
    public GameObject spikes;
    public PlayerController pc;
    public float duration = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        Debug.Log("Gate down");
        Vector3 checkpointLocation = new Vector3(0,spikes.transform.position.y + 2,0);
        pc.UpdateSpawnerLocation(checkpointLocation);
        StartCoroutine(Retract());
    }
    private IEnumerator Retract()
    {
        float elapsedTime = 0;
        Vector3 startingScale = spikes.transform.localScale;
        Vector3 targetScale = new Vector3(100.0f, 100.0f, 0.0f);
        while (elapsedTime < duration)
        {
            spikes.transform.localScale = Vector3.Lerp(startingScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            //Debug.Log(spikes.transform.localScale);
            yield return null;
        }
    }
}
