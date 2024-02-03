using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeightMeter : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Slider>().value = player.transform.position.y;
    }
}
