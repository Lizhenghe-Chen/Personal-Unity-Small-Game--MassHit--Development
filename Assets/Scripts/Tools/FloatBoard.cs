using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatBoard : MonoBehaviour
{
    public bool isParent = false;//if the object is parent, need to check the child is in their position
    public bool isVertical = false;
    public Vector3 originialPos, originalRot, VericalOffset = new Vector3(0, 2, 0);
    [SerializeField] Material floatMaterial, VerticalFloatMaterial;

    [SerializeField] Rigidbody rb;
    private void OnValidate()
    {
        if (!isParent)
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

    }
    // Start is called before the first frame update
    void Awake()
    {
        if (isParent) StartCoroutine(ChildReturnToOriginPosSelfCheck());
        else
        {
            rb = GetComponent<Rigidbody>();
            originialPos = transform.position;
            originalRot = transform.eulerAngles;
        }
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
    public IEnumerator ReturnToOriginPos()
    {
        while ((transform.position - originialPos).magnitude > 0.001f)
        {
            this.rb.isKinematic = true;
            // this.speed = ((transform.position - originialPos).magnitude < 0.1f) ? 0.5f : Time.deltaTime;
            speed = Time.deltaTime;
            if ((transform.position - originialPos).magnitude <= 0.1f)
            {
                transform.position = originialPos;
                transform.eulerAngles = originalRot;
            }
            this.rb.MovePosition(Vector3.Lerp(transform.position, originialPos, speed));
            this.rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(originalRot), speed));
            //transform.eulerAngles = originalRot;
            yield return new WaitForFixedUpdate();
        }
        this.rb.isKinematic = false;
        Debug.Log("Returned to origin pos");
        this.StopCoroutine(ReturnToOriginPos());
    }
    IEnumerator ChildReturnToOriginPosSelfCheck()
    {
        while (true)
        {
            foreach (Transform child in transform)
            {

                var childFloatBoard = child.GetComponent<FloatBoard>();
                if (child.position != childFloatBoard.originialPos) { Debug.Log("Not in origin pos"); childFloatBoard.StartCoroutine(childFloatBoard.ReturnToOriginPos()); }


            }
            yield return new WaitForSeconds(10f);
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
