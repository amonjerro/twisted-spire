using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeKickHandler : MonoBehaviour, IKickableTarget
{
    public IActivatable objectToActivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnKicked()
    {
        objectToActivate.Activate();
    }
}
