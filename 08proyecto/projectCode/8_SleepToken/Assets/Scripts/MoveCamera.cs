using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MoveCamera : MonoBehaviour
{
    public int band;
    public float startScale, scaleMultiplier;
    CinemachineVirtualCamera cinemachine;

    void Start()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = (AudioManager.bandBuffer[0] * scaleMultiplier) + startScale;        
    }
}