using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickableTarget : MonoBehaviour
{
    public KickableActivated dependent;
    public void ActivateDependent()
    {
        if (dependent != null)
        {
            dependent.Open();
        }   
    }
}
