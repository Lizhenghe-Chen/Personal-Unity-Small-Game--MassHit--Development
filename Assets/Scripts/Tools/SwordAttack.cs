using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Copyright (c) [2023] [Lizhneghe.Chen https://github.com/Lizhenghe-Chen]
* Please do not use these code directly without permission.
*/
public class SwordAttack : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Sword hit " + other.gameObject.name);
        //Push Object in Opposite Direction of Collision
        other.gameObject.GetComponent<Rigidbody>().AddForce(-other.contacts[0].normal * 1000);
    }
}
