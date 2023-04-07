using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Copyright (c) [2023] [Lizhneghe.Chen https://github.com/Lizhenghe-Chen]
* Please do not use these code directly without permission.
*/
public class JumpPad : MonoBehaviour
{
    public Animator JumpPadAnimation;
    public AudioSource JumpPadAudio;
    public float JumpForce = 10;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody)
        {
            JumpPadAudio.Play(0);
            JumpPadAnimation.Play("PlayBounce", 0, 0);
            collision.rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            JumpPadAudio.Play(0);
            JumpPadAnimation.Play("PlayBounce", 0, 0);
            other.attachedRigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }
}
