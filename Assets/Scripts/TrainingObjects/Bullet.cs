using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float flySpeed = 4f;
    private Shooter shooter;

    public Vector3 Direction { get { return -transform.right; } }

    public void Fly(Shooter sh)
    {
        shooter = sh;
        StartCoroutine(DoFly());
    }

    private IEnumerator DoFly()
    {
        while (true)
        {
            transform.position += -transform.right * (flySpeed * Time.deltaTime);
            yield return true;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.rigidbody != null)
        {
            if (col.rigidbody.CompareTag("agent"))
            {
                var mForce = -transform.right * 20000;
                col.rigidbody.AddForceAtPosition(mForce, col.GetContact(0).point);
                shooter.HitAgent();
            }
        }
        else if (col.collider.CompareTag("ground"))
        {
            shooter.HitWall();
        }
    }
}
