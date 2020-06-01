using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCinemachineState : MonoBehaviour
{
    // Start is called before the first frame update
    public CinemachineVirtualCamera vcam;



    public void SetToZero()
    {
        CinemachineFramingTransposer trans = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();


        trans.m_XDamping = 0;
        trans.m_YDamping = 0;
        trans.m_ZDamping = 0;
    }
    public void ReturnToNormal()
    {
        CinemachineFramingTransposer trans = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        trans.m_XDamping = 1;
        trans.m_YDamping = 1;
        trans.m_ZDamping = 1;
    }
}
