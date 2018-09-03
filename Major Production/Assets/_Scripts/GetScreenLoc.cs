using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetScreenLoc { 

    public Vector2 getScreenPos(Transform targetScreenPos)
    {
        Vector3 temp = Camera.main.WorldToScreenPoint(targetScreenPos.position);
        return new Vector2(temp.x, temp.y);
    }

}
