using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
//This File shoulbe attach to Camera
public class CameraDistSecond : MonoBehaviour
{
    [Header("Need Assign in Inspector\n")]
    [SerializeField] private Volume postProcessVolume;
    [SerializeField] Transform Player;
    //[SerializeField] CinemachineVirtualCamera virtualCamera;
    public float maxRadius = -40; //should <0!!!!!
    public float minRadius = 0; //should ==0

    [SerializeField] float disdance; //for postProcessVolume Depth od Field use
    [SerializeField] float playerRadius;
    private CinemachineTransposer transposer;
    readonly int offset_Value = 2; //offset when Mouse ScrollWheel

    private DepthOfField dof;

    [SerializeField] float total_Offset_Z, smoothSpeed = 2f;//distance between player and camera


    void Start()
    {
        transposer = this.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineOrbitalTransposer>();
        total_Offset_Z = transposer.m_FollowOffset.z;
        transposer.m_FollowOffset = new Vector3(0, -0.3f * total_Offset_Z, total_Offset_Z);
        postProcessVolume.profile.TryGet<DepthOfField>(out dof);

    }
    void Update()
    {
        ScrollWheeldetect();

    }
    private void FixedUpdate()
    {
        ChangeDepthOfField();
        SoftMoveCamera();
    }

    void ScrollWheeldetect()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && total_Offset_Z > maxRadius)
        {
            total_Offset_Z -= offset_Value;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && total_Offset_Z < minRadius)
        {
            total_Offset_Z += offset_Value;
        }

    }
    void SoftMoveCamera()
    {
        if (transposer.m_FollowOffset.z == total_Offset_Z) { return; }
        var target = Mathf.Lerp(transposer.m_FollowOffset.z, total_Offset_Z, Time.unscaledDeltaTime * smoothSpeed);
        transposer.m_FollowOffset = new Vector3(0, -0.3f * target, target);
    }
    void ChangeDepthOfField()

    {//https://github.com/keijiro/PostProcessingUtilities/blob/master/Assets/Runtime/FocusPuller.cs
        disdance = Vector3.Distance(Player.position, transform.position);
        dof.focusDistance.value = disdance;
    }


}
