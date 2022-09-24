using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatBoard : MonoBehaviour
{
    public bool isVertical = false;
    [SerializeField] Material floatMaterial, VerticalFloatMaterial;
    [SerializeField] private Vector3 originialPos, originalRot, VericalOffset = new Vector3(0, 2, 0);
    [SerializeField] Rigidbody rb;
    private void OnValidate()
    {
        rb = GetComponent<Rigidbody>();
        if (isVertical)
        {
            SetToVerticalFloat();
        }
        else
        {
            SetToFloat();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originialPos = transform.position;
        originalRot = transform.eulerAngles;
        StartCoroutine(ReturnToOriginPosSelfCheck());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isVertical) Invoke("InvokeReturnToOriginPos", 3f);
    }
    private void OnCollisionStay(Collision other)
    {
        if (isVertical)
        {
            rb.isKinematic = true;
            transform.position = Vector3.Lerp(transform.position, originialPos + VericalOffset, Time.deltaTime);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (isVertical) { Invoke("InvokeReturnToOriginPos", 3f); }
    }
    private void InvokeReturnToOriginPos()
    {
        StartCoroutine(ReturnToOriginPos());
    }
    float speed = 1f;
    IEnumerator ReturnToOriginPos()
    {
        rb.isKinematic = true;
        while (transform.position != originialPos)
        {
            speed = ((transform.position - originialPos).magnitude < 0.1f) ? 0.5f : Time.deltaTime;
            rb.MovePosition(Vector3.Lerp(transform.position, originialPos, speed));
            rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(originalRot), speed));
            //transform.eulerAngles = originalRot;
            yield return new WaitForFixedUpdate();
        }
        rb.isKinematic = false;
        Debug.Log("Returned to origin pos");
        StopCoroutine(ReturnToOriginPos());
    }
    IEnumerator ReturnToOriginPosSelfCheck()
    {
        while (true)
        {
            if (transform.position != originialPos) { Debug.Log("Not in origin pos"); StartCoroutine(ReturnToOriginPos()); }
            yield return new WaitForSeconds(5f);
        }
    }
    private void SetToFloat()
    {
        rb.freezeRotation = false;
        this.GetComponent<MeshRenderer>().material = floatMaterial;
    }
    private void SetToVerticalFloat()
    {
        rb.freezeRotation = true;
        this.GetComponent<MeshRenderer>().material = VerticalFloatMaterial;
    }
}
