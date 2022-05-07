using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
//This File shoulbe attach to Camera
public class cameraDist : MonoBehaviour
{
    [Header("**Below Parameters should find by themsleves at the Start()**\n")]
    [SerializeField] private Volume postProcessVolume;


    [SerializeField] Transform playerKernel;
    [SerializeField] float disdance; //for postProcessVolume Depth od Field use
    [SerializeField] float playerRadius;
    float maxRadius = 10; //should >0
    float minRadius = -2; //should <0
    int offset_Value = 1; //offset when Mouse ScrollWheel

    private DepthOfField dof;
    private CinemachineFreeLook virtualCamera;
    [SerializeField] float total_Offset, smoothSpeed = 2f;//distance between player and camera
    [SerializeField] float[] currentRadius = { 0, 0, 0 }, currentHeight = { 0, 0, 0 };//the size should same as virtualCamera.m_Orbits.Length


    void Start()
    {
        virtualCamera = this.GetComponent<CinemachineFreeLook>();
        postProcessVolume.profile.TryGet<DepthOfField>(out dof);
        // player = transform.parent;

        // playerRadius = player.GetComponent<SphereCollider>().radius;
        for (int i = 0; i < virtualCamera.m_Orbits.Length; i++)
        {
            currentRadius[i] = virtualCamera.m_Orbits[i].m_Radius;
            currentHeight[i] = virtualCamera.m_Orbits[i].m_Height;
        }



    }
    void Update()
    {
        if (playerKernel == null) return;
        ScrollWheeldetect();
        ChangeDepthOfField();
        if (virtualCamera.m_Orbits[1].m_Radius != currentRadius[1] && virtualCamera.m_Orbits[1].m_Height != currentHeight[1]) { SoftMoveCamera(); }

    }

    void ScrollWheeldetect()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && total_Offset < maxRadius)
        {
            //SetDistance(1);
            currentRadius[0] += offset_Value * 0.1f;
            currentRadius[1] += offset_Value;
            currentRadius[2] += offset_Value;
            for (int i = 0; i < currentRadius.Length - 1; i++)
            {
                //currentRadius[i] += offset_Value;
                currentHeight[i] += offset_Value;
            }
            total_Offset += offset_Value;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && total_Offset > minRadius)
        {
            currentRadius[0] -= offset_Value * 0.1f;
            currentRadius[1] -= offset_Value;
            currentRadius[2] -= offset_Value;
            for (int i = 0; i < currentRadius.Length - 1; i++)
            {
                //currentRadius[i] -= offset_Value;
                currentHeight[i] -= offset_Value;
            }
            total_Offset -= offset_Value;
            //SetDistance(-1);
        }

    }
    void SoftMoveCamera()
    {
        for (int i = 0; i < virtualCamera.m_Orbits.Length; i++)
        {
            virtualCamera.m_Orbits[i].m_Radius = Mathf.Lerp(virtualCamera.m_Orbits[i].m_Radius, currentRadius[i], Time.deltaTime * smoothSpeed);
            virtualCamera.m_Orbits[i].m_Height = Mathf.Lerp(virtualCamera.m_Orbits[i].m_Height, currentHeight[i], Time.deltaTime * smoothSpeed);
        }
    }
    void ChangeDepthOfField()

    {//https://github.com/keijiro/PostProcessingUtilities/blob/master/Assets/Runtime/FocusPuller.cs
        disdance = Vector3.Distance(playerKernel.position, transform.position);
        dof.focusDistance.value = disdance;
    }
    // void SetDistance(int direction)
    // {
    //     for (int i = 0; i < virtualCamera.m_Orbits.Length; i++)
    //     {
    //         virtualCamera.m_Orbits[i].m_Radius += direction * offset_Value;
    //         total_Offset += direction * offset_Value;
    //     }
    // }

}
