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

        Speed = Input.GetKey(GlobalRules.instance.SpeedUp) ? moveSpeed * 10 : moveSpeed;
        Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit);
        // Debug.Log(Camera.forward);
        if (hit.distance < 0.5)
        {
            //Speed = moveSpeed;
            if (hit.distance < 0.2)
            {
                Speed = moveSpeed;
                transform.position += 0.1f * Vector3.up;
                transform.position += Speed * Time.unscaledDeltaTime * new Vector3(Camera.forward.x, 0, Camera.forward.z) * vertical;
                transform.position += Speed * Time.unscaledDeltaTime * new Vector3(Camera.forward.x, 0, Camera.forward.z) * horizontal;

            }
            else
            {
                // Speed = Input.GetKey(GlobalRules.instance.SpeedUp) ? moveSpeed * 10 : moveSpeed;
                transform.position += Speed * Time.unscaledDeltaTime * Camera.forward * vertical;
                transform.position += Speed * Time.unscaledDeltaTime * Camera.right * horizontal;
                // if (Input.GetKey(KeyCode.Q)) { transform.position -= moveSpeed * Time.unscaledDeltaTime * Vector3.up; }
            }
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
