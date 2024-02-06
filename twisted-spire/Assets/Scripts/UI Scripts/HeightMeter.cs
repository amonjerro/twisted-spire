using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeightMeter : MonoBehaviour
{
    public GameObject player;
    public GameObject towerTop;

    void Start()
    {
        //Set the max height of the meter to the top of the tower
        this.GetComponent<Slider>().maxValue = towerTop.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Slider>().value = player.transform.position.y;
    }
}
