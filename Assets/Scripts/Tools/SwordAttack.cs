using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Sword hit " + other.gameObject.name);
        //Push Object in Opposite Direction of Collision
        other.gameObject.GetComponent<Rigidbody>().AddForce(-other.contacts[0].normal * 1000);
    }
}
