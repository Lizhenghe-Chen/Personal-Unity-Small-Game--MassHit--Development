using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCtrl : MonoBehaviour
{
    public Transform Camera;
    public float moveSpeed = 5f;


    private float horizontalInput, verticalInput;
    private Rigidbody rb; // player's rigidbody
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Movement();
    }
    void Movement()
    {
        var Speed = Input.GetKey(GlobalRules.instance.SpeedUp) ? moveSpeed * 10 : moveSpeed;
        rb.AddForce(Speed * verticalInput * Camera.forward);
        rb.AddForce(Speed * horizontalInput * Camera.right);
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.E))
        {
            rb.AddForce(Vector3.up * Speed);

            //  Debug.Log("Space pressed and UP");
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rb.AddForce(-Vector3.up * Speed);

            //   Debug.Log(a + "Space pressed and jump" + jumpCount);
        }
    }
}
