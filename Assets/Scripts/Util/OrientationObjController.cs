using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationObjController : MonoBehaviour
{
    //Update position and Rotation
    public void UpdateOrientation(Transform rootBP, Transform target)
    {
        var dirVector = target.position - transform.position;
        //dirVector.y = 0; //flatten dir on the y. this will only work on level, uneven surfaces
        var lookRot =
            dirVector == Vector3.zero
                ? Quaternion.identity
                : Quaternion.LookRotation(dirVector); //get our look rot to the target

        //UPDATE ORIENTATION CUBE POS & ROT
        transform.SetPositionAndRotation(rootBP.position, lookRot);
    }
}
