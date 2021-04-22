using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPingPonger : MonoBehaviour
{
    [SerializeField]
    private Vector3 startPos;
    [SerializeField]
    private Vector3 endPos;

    private float time;

    private void Start()
    {
        if(startPos == Vector3.zero)
        {
            startPos = transform.localPosition;
        }
    }

    void Update()
    {
        var moveSpeed = SpectrumManager.Instance.MetalFactor;

        if (!SpectrumManager.Instance.IsMetal())
        {
            moveSpeed *= 0.5f;
        }

        time += Time.deltaTime * moveSpeed;
        transform.localPosition = Vector3.Lerp(startPos, endPos, Mathf.PingPong(time, 1));
    }
}
