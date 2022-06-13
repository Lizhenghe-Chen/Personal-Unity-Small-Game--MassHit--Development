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
        // Movement();
        MovementUnscaled();
    }


    void Movement()
    {
        var Speed = Input.GetKey(GlobalRules.instance.SpeedUp) ? moveSpeed * 10 : moveSpeed;
        rb.AddForce(Speed * Time.unscaledDeltaTime * verticalInput * Camera.forward);
        rb.AddForce(horizontalInput * Speed * Time.unscaledDeltaTime * Camera.right);
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
    void MovementUnscaled()
    {
        if (Input.GetKey(KeyCode.W)) { transform.position += Camera.forward * moveSpeed * Time.unscaledDeltaTime; }
        if (Input.GetKey(KeyCode.S)) { transform.position -= Camera.forward * moveSpeed * Time.unscaledDeltaTime; }
        if (Input.GetKey(KeyCode.A)) { transform.position -= Camera.right * moveSpeed * Time.unscaledDeltaTime; }
        if (Input.GetKey(KeyCode.D)) { transform.position += Camera.right * moveSpeed * Time.unscaledDeltaTime; }
        if (Input.GetKey(KeyCode.Space)) { transform.position += Vector3.up * moveSpeed * Time.unscaledDeltaTime; }
        if (Input.GetKey(KeyCode.Q)) { transform.position -= Vector3.up * moveSpeed * Time.unscaledDeltaTime; }
        if (Input.GetKey(KeyCode.E)) { transform.position += Vector3.up * moveSpeed * Time.unscaledDeltaTime; }



    }
}
