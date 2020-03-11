using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed=0.125f;
    public float DefaultSmoothSpeed = 0.125f;
    public Vector3 offset;
    public PlayerMovement playerMovement;

    private void FixedUpdate()
    {
       
        Vector3 vel = Vector3.zero;
        if (playerMovement.currentlyLookingAt == PlayerMovement.lookingAt.down)
        {
            offset.y = -1;
        }
        if (playerMovement.currentlyLookingAt == PlayerMovement.lookingAt.up)
        {
            offset.y = 1;
        }
        if (playerMovement.currentlyLookingAt == PlayerMovement.lookingAt.left)
        {
            offset.x = -1;
        }
        if (playerMovement.currentlyLookingAt == PlayerMovement.lookingAt.right)
        {
            offset.x = 1;
        }
        transform.position = Vector3.SmoothDamp(transform.position,target.position + offset, ref vel,smoothSpeed);
    }
}
