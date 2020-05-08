using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    // Start is called before the first frame update
    CinemachineVirtualCamera cinemachine;
    CinemachineBasicMultiChannelPerlin perlin;

    float shakeDuration = 0.0f;
    void Start()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        perlin = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            shakeDuration -= Time.deltaTime;

        }
        else
        {
            perlin.m_AmplitudeGain = 0;

        }

    }

    public void ShakeCameraOnArrowHit()
    {
        shakeDuration = 0.05f;
        perlin.m_AmplitudeGain = 1;
    }   
}
