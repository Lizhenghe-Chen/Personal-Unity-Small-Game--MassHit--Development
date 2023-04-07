using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
/* Copyright (c) [2023] [Lizhneghe.Chen https://github.com/Lizhenghe-Chen]
* Please do not use these code directly without permission.
*/
public class SelfRotate : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] bool rotateX = false, rotateY = false, rotateZ = false;
    private Quaternion deltaRotation;
    private Vector3 m_EulerAngleVelocity;
    private void OnValidate()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        if (rotateX)
        {
            rotateY = false;
            rotateZ = false;
        }
        if (rotateY)
        {
            rotateX = false;
            rotateZ = false;
        }
        if (rotateZ)
        {
            rotateX = false;
            rotateY = false;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //https://docs.unity3d.com/ScriptReference/Rigidbody.MoveRotation.html
    void FixedUpdate()
    {
        if (!rotateX && !rotateY && !rotateZ) { return; }
        if (rotateX)
        {
            m_EulerAngleVelocity = new Vector3(1, 0, 0);
        }
        else if (rotateY)
        {
            m_EulerAngleVelocity = new Vector3(0, 1, 0);
        }
        else if (rotateZ)
        {
            m_EulerAngleVelocity = new Vector3(0, 0, 1);
        }
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime * rotateSpeed);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
