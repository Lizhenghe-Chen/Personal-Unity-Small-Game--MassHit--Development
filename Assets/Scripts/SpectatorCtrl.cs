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
        if (Time.timeScale >= 0.5) { Movement(); } else MovementUnscaled();
        if (Input.GetKey(GlobalRules.instance.Break)) { rb.velocity = Vector3.zero; }


    }


    void Movement()
    {
        var Speed = Input.GetKey(GlobalRules.instance.SpeedUp) ? moveSpeed * 10 : moveSpeed;
        rb.AddForce(Speed * verticalInput * Camera.forward);
        rb.AddForce(horizontalInput * Speed * Camera.right);
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

    float Speed, vertical, horizontal;
    void MovementUnscaled()
    {
        if (Input.GetKey(GlobalRules.instance.Break)) { return; }
        if (Input.GetKey(KeyCode.W))
        {
            vertical = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            vertical = -1;
        }
        else { vertical = 0; }

        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1;
        }
        else { horizontal = 0; }

        Speed = Input.GetKey(GlobalRules.instance.SpeedUp) ? moveSpeed * 5 : moveSpeed;
        Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit);
        //Debug.Log(hit.distance);
        if (hit.distance < 0.2)
        {
            if (hit.distance < 0.1) { transform.position += new Vector3(0, 0.001f, 0) + hit.distance * Vector3.up; }

            // Speed = Input.GetKey(GlobalRules.instance.SpeedUp) ? moveSpeed * 10 : moveSpeed;
            transform.position += Speed * Time.unscaledDeltaTime * Camera.forward * vertical;
            transform.position += Speed * Time.unscaledDeltaTime * Camera.right * horizontal;
            // if (Input.GetKey(KeyCode.Q)) { transform.position -= moveSpeed * Time.unscaledDeltaTime * Vector3.up; }

        }
        else
        {

            transform.position += Speed * Time.unscaledDeltaTime * Camera.forward * vertical;
            transform.position += Speed * Time.unscaledDeltaTime * Camera.right * horizontal;
            //transform.position += Speed * Time.unscaledDeltaTime * verticalInput * Camera.forward;
            //transform.position += Speed * Time.unscaledDeltaTime * horizontalInput * Camera.right;
            if (Input.GetKey(GlobalRules.instance.MoveDown)) { transform.position -= Speed * Time.unscaledDeltaTime * Vector3.up; }
        }
        if (Input.GetKey(GlobalRules.instance.MoveUp) || Input.GetKey(GlobalRules.instance.Jump)) { transform.position += Speed * Time.unscaledDeltaTime * Vector3.up; }



    }

}
