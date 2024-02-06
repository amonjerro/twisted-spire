using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Camera me;
    // Start is called before the first frame update
    void Start()
    {
        me = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            me.orthographic = !me.orthographic;
        }
    }
}
