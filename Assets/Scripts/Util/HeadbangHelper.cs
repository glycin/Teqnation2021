using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadbangHelper : MonoBehaviour
{
    [SerializeField]
    private Vector3 maxPos;
    [SerializeField]
    private Vector3 minPos;
    [SerializeField]
    private float moveSpeed = 1f;

    private Vector3 neutralPos;
    private float time;

    private void Awake()
    {
        neutralPos = Vector3.Lerp(maxPos, minPos, 0.5f);
        Reset();
    }

    public void Reset()
    {
        //transform.localPosition = Vector3.Lerp(maxPos, minPos, Random.Range(0f, 1f));
    }

    void FixedUpdate()
    {
        moveSpeed = SpectrumManager.Instance.MetalFactor;

        if (SpectrumManager.Instance.IsMetal())
        {
            //moveSpeed *= 2f;
        }
        else
        {
            moveSpeed *= 0.5f;
        }

        time += Time.deltaTime * moveSpeed;
        transform.localPosition = Vector3.Lerp(minPos, maxPos, Mathf.PingPong(time, 1));        
    }
}
