using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public enum relaxingLookingDirection { down = 0, up = 1, left = 2, right = 3 };

    public bool randomLookLocationDuringRelaxTime;
    public relaxingLookingDirection lookDirDuringRelaxTime;

}
