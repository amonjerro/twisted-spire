 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera camera;

    private void Awake()
    {
        camera = GameObject.FindFirstObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform.position);
    }
}
