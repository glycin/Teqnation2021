using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class SnitchAgent : Agent
{
    [SerializeField]
    private Transform seeker1;

    [SerializeField]
    private Transform seeker2;

    [SerializeField]
    private Transform[] walls;

    private float flySpeed = 9.9f;

    private float previousStepDistanceTo1;
    private float currentStepDistanceTo1;

    private float previousStepDistanceTo2;
    private float currentStepDistanceTo2;

    private Vector3 startPos;
    private Vector3 startPos1;
    private Vector3 startPos2;

    private void Start()
    {
        startPos = transform.localPosition;
        startPos1 = seeker1.localPosition;
        startPos2 = seeker2.localPosition;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var moveX = actions.ContinuousActions[0];
        var moveZ = actions.ContinuousActions[1];
        var moveY = actions.ContinuousActions[2];

        transform.localPosition += transform.forward * (moveX * flySpeed * Time.deltaTime);
        transform.localPosition += transform.right * (moveZ * flySpeed * Time.deltaTime);
        transform.localPosition += transform.up * (moveY * flySpeed * Time.deltaTime);

        if (currentStepDistanceTo1 > previousStepDistanceTo1)
        {
            SetReward(0.1f);
        }

        if (currentStepDistanceTo2 > previousStepDistanceTo2)
        {
            SetReward(0.1f);
        }

        previousStepDistanceTo1 = currentStepDistanceTo1;
        previousStepDistanceTo2 = currentStepDistanceTo2;

        if(StepCount == MaxStep - 1)
        {
            SetReward(1f);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(seeker1.localPosition);
        sensor.AddObservation(seeker2.localPosition);
        currentStepDistanceTo1 = Vector3.Distance(transform.localPosition, seeker1.localPosition);
        sensor.AddObservation(currentStepDistanceTo1);
        currentStepDistanceTo2 = Vector3.Distance(transform.localPosition, seeker2.localPosition);
        sensor.AddObservation(currentStepDistanceTo2);

        foreach(var w in walls)
        {
            sensor.AddObservation(w.localPosition);
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startPos;
        seeker1.transform.localPosition = startPos1;
        seeker2.transform.localPosition = startPos2;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            SetReward(-1f);
            EndEpisode();
        }
        else
        {
            SetReward(-0.1f);
        }
    }

    public void End()
    {
        SetReward(-1f);
        EndEpisode();
    }
}
