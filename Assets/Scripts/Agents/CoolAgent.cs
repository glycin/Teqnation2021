using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Collections;
using TMPro;
public class CoolAgent : Agent
{
    [SerializeField]
    private BarrelSpawner spawner;
    [SerializeField]
    private TextMeshPro text;

    private ExplosiveBarrel barrel;
    private float rotateSpeed = 250f;

    public override void OnActionReceived(ActionBuffers actions)
    {
        var rotY = actions.ContinuousActions[0];
        transform.Rotate(transform.up, rotY * rotateSpeed * Time.deltaTime);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(barrel.transform.localPosition);
    }

    public override void OnEpisodeBegin()
    {
        //transform.localPosition = new Vector3(Random.Range(-4f, 3f), -0.3f, Random.Range(0f, -6f)); ;
        //transform.Rotate(transform.up, Random.Range(0, 360));
        barrel = spawner.SpawnBarrel();
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(5.0f);
        barrel.Explode();
        var score = -IsLookingAt(barrel.transform);
        SetReward(score);

        #region Debug
        if (score > 0)
        {
            text.color = Color.green;
            text.text = $"COOL {score}";
        }
        else
        {
            text.color = Color.red;
            text.text = $"UNCOOL {score}";
        }
        #endregion
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxisRaw("Vertical");
        IsLookingAt(barrel.transform);
    }

    private float IsLookingAt(Transform target)
    {
        var dir = (target.localPosition - transform.localPosition).normalized;
        var dot = Vector3.Dot(dir, transform.forward);
        return dot;
    }

#region Animation Events
    public void FootR(int step)
    {
        // Do nothing
    }

    public void FootL(int step)
    {
        // Do nothing
    }
#endregion
}
