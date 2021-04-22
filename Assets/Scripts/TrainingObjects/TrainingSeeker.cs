using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingSeeker : MonoBehaviour
{
    public Transform snitch;
    private float flySpeed = 9f;

    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, snitch.localPosition, flySpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Goal")
        {
            collision.transform.GetComponent<SnitchAgent>().End();
        }
    }
}
