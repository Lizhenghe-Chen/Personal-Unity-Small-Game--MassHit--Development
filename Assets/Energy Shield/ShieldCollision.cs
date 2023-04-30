using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    [ColorUsage(true, true)] public Color interactionColor;
    private void OnCollisionEnter(Collision other)
    {
        //get the other object's emissive color
        Renderer rend = other.gameObject.GetComponent<Renderer>();
        if (rend != null)
        {
            interactionColor = rend.material.GetColor("_EmissionColor");
            //change the interactionColor intensity to 2
            interactionColor *= 100f;
            
        }
                Shield.instance.AddInteractionData(other.contacts[0].point, interactionColor);
    }
}
