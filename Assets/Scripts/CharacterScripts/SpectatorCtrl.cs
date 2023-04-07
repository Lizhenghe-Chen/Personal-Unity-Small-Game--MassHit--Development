using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Copyright (c) [2023] [Lizhneghe.Chen https://github.com/Lizhenghe-Chen]
* Please do not use these code directly without permission.
*/
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
    private void OnEnable()
    {
        transform.position = Camera.position;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (Time.timeScale >= 0.7f) { Movement(); } else MovementUnscaled();
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
        if (Input.GetKey(GlobalRules.instance.MoveDown)) { transform.position -= Speed * Time.unscaledDeltaTime * Vector3.up; }
        if (Input.GetKey(GlobalRules.instance.MoveUp) || Input.GetKey(GlobalRules.instance.Jump)) { transform.position += Speed * Time.unscaledDeltaTime * Vector3.up; }
        transform.position += Speed * Time.unscaledDeltaTime * vertical * Camera.forward;
        transform.position += horizontal * Speed * Time.unscaledDeltaTime * Camera.right;
        Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit);
        //raycast
//        Debug.Log(hit.distance);
        if (hit.distance < 0.1 || hit.distance == Mathf.Infinity)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, 1, 0), Time.unscaledDeltaTime);
        }
    }
}
