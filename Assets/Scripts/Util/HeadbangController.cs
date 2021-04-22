using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadbangController : MonoBehaviour
{
    [SerializeField]
    private MetalheadOrientationHelper orientationHelper;

    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, orientationHelper.transform.localEulerAngles.x);
    }
}
