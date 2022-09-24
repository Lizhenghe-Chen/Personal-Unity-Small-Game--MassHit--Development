using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterWithCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Image WaterMask;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WaterMask.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WaterMask.enabled = false;
        }
    }
}
