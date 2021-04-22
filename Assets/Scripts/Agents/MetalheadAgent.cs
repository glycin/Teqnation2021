using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MetalheadAgent : Agent
{
    [SerializeField]
    private MetalheadOrientationHelper orientationHelper;

    [SerializeField]
    private HeadbangHelper helper;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(orientationHelper.transform.localRotation);
        sensor.AddObservation(orientationHelper.transform.localPosition);
        sensor.AddObservation(SpectrumManager.Instance.MetalFactor);
        sensor.AddObservation(SpectrumManager.Instance.IsMetal());
        sensor.AddObservation(helper.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var acts = actions.ContinuousActions;
        orientationHelper.Rotate(acts[0]);
    }

    public override void OnEpisodeBegin()
    {
        orientationHelper.Reset();
        helper.Reset();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;
        var actionNum = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            actionNum = 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            actionNum = -1f;
        }

        actions[0] = actionNum;
        actions[1] = actionNum;
        actions[2] = actionNum;
    }

    private void FixedUpdate()
    {
        var direction = (helper.transform.localPosition - orientationHelper.transform.localPosition).normalized;
        //Debug.DrawRay(orientationHelper.transform.position, direction * 5, Color.red);
        var dot = Vector3.Dot(direction, orientationHelper.transform.forward);
        if (dot < -0.95f)
        {
            SetReward(-1f);
            EndEpisode();
        }

        if (dot >= 0.99f)
        {
            SetReward(1f);
            EndEpisode();
        }
    }
}
