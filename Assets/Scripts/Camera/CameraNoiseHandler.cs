using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNoiseHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] cinemachineThirdPersonCameras;

    private void Update()
    {
        foreach (CinemachineVirtualCamera cam in cinemachineThirdPersonCameras)
        {
            //cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1;
        }
    }
}
