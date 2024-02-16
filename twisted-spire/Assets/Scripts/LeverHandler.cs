using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHandler : MonoBehaviour, IKickableTarget
{
    public GameObject spikes;
    public GameObject lever;
    Vector3 scale = new Vector3(1f, 1f, 0f);
    Vector3 rotation = new Vector3(0, 0, 180);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnKicked()
    {
        lever.transform.Rotate(rotation);
        if (spikes.TryGetComponent(out IActivatable gate))
        {
            gate.Activate();
        }
        //gate.Activate();
    }
}
