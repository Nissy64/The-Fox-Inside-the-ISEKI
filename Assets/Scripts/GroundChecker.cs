using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public static bool IsGround(Transform position, Vector2 size, LayerMask layer)
    {
        if(Physics2D.OverlapBox(position.position, size, 0, layer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
