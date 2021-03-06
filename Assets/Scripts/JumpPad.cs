using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public Animator JumpPadAnimation;
    public AudioSource JumpPadAudio;
    public float JumpForce = 10;


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.rigidbody)
    //    {
    //        JumpPadAnimation.Play("PlayBounce", 0, 0);
    //        collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    //    }
    //}
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
