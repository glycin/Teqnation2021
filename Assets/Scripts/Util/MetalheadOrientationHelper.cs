using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalheadOrientationHelper : MonoBehaviour
{
    private Quaternion startRotation;

    private void Awake()
    {
        startRotation = transform.localRotation;
    }

    public void Rotate(float rotValue)
    {
        transform.Rotate(-transform.right, rotValue * 5);
    }

    public void Reset()
    {
        //transform.localRotation = startRotation;
    }
}
