using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class NeoAgent : Agent
{
    [Header("Body Parts")] 
    public Transform hips;
    public Transform chest;
    public Transform head;
    public Transform shoulderL;
    public Transform shoulderR;
    public Transform armL;
    public Transform armR;
    public Transform upperLegL;
    public Transform upperLegR;
    public Transform lowerLegL;
    public Transform lowerLegR;

    [SerializeField]
    public Shooter shooter;

    [SerializeField]
    public Transform dodgeLookTarget;

    private JointDriveController jdController;
    private OrientationObjController orientationObj;

    public override void Initialize()
    {
        orientationObj = GetComponentInChildren<OrientationObjController>();
        jdController = GetComponent<JointDriveController>();
        jdController.SetupBodyPart(hips);
        jdController.SetupBodyPart(chest);
        jdController.SetupBodyPart(shoulderL);
        jdController.SetupBodyPart(shoulderR);
        jdController.SetupBodyPart(armL);
        jdController.SetupBodyPart(armR);
        jdController.SetupBodyPart(upperLegL);
        jdController.SetupBodyPart(upperLegR);
        jdController.SetupBodyPart(lowerLegL);
        jdController.SetupBodyPart(lowerLegR);
        jdController.SetupBodyPart(head);
    }

    public override void OnEpisodeBegin()
    {
        //Reset all of the body parts
        foreach (var bodyPart in jdController.bodyPartsDict.Values)
        {
            bodyPart.Reset(bodyPart);
        }

        shooter.Reset();
    }

    public override void CollectObservations(VectorSensor sensor)
    {

        var cubeForward = orientationObj.transform.forward;

        var avgVel = GetAvgVelocity();

        sensor.AddObservation(Vector3.Distance(cubeForward, avgVel));
        //avg body vel relative to cube
        sensor.AddObservation(orientationObj.transform.InverseTransformDirection(avgVel));
        //vel goal relative to cube
        sensor.AddObservation(orientationObj.transform.InverseTransformDirection(cubeForward));

        //rotation deltas
        sensor.AddObservation(Quaternion.FromToRotation(hips.forward, cubeForward));
        sensor.AddObservation(Quaternion.FromToRotation(head.forward, cubeForward));

        //Position of target position relative to cube
        sensor.AddObservation(orientationObj.transform.InverseTransformPoint(shooter.GunPosition));

        foreach (var bodyPart in jdController.bodyPartsList)
        {
            CollectObservationBodyPart(bodyPart, sensor);
        }

        var bullet = shooter.GetBullets();
        var bulletVectorObservation = Vector3.zero;

        sensor.AddObservation(bullet == null);

        if (bullet == null)
        {
            bulletVectorObservation = orientationObj.transform.InverseTransformPoint(shooter.GunPosition);
        }
        else
        {
            bulletVectorObservation = orientationObj.transform.InverseTransformPoint(bullet.transform.position);
        }

        sensor.AddObservation(bulletVectorObservation);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)

    {
        var bpDict = jdController.bodyPartsDict;
        var i = -1;

        var continuousActions = actionBuffers.ContinuousActions;
        bpDict[chest].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], continuousActions[++i]);

        bpDict[upperLegL].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);
        bpDict[upperLegR].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);
        bpDict[lowerLegL].SetJointTargetRotation(continuousActions[++i], 0, 0);
        bpDict[lowerLegR].SetJointTargetRotation(continuousActions[++i], 0, 0);

        bpDict[armL].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);
        bpDict[armR].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);
        bpDict[shoulderL].SetJointTargetRotation(continuousActions[++i], 0, 0);
        bpDict[shoulderR].SetJointTargetRotation(continuousActions[++i], 0, 0);
        bpDict[head].SetJointTargetRotation(continuousActions[++i], continuousActions[++i], 0);

        //update joint strength settings
        bpDict[chest].SetJointStrength(continuousActions[++i]);
        bpDict[head].SetJointStrength(continuousActions[++i]);
        bpDict[upperLegL].SetJointStrength(continuousActions[++i]);
        bpDict[lowerLegL].SetJointStrength(continuousActions[++i]);
        bpDict[upperLegR].SetJointStrength(continuousActions[++i]);
        bpDict[lowerLegR].SetJointStrength(continuousActions[++i]);
        bpDict[armL].SetJointStrength(continuousActions[++i]);
        bpDict[shoulderL].SetJointStrength(continuousActions[++i]);
        bpDict[armR].SetJointStrength(continuousActions[++i]);
        bpDict[shoulderR].SetJointStrength(continuousActions[++i]);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;

        for(var i = 0; i < 27; ++i)
        {
            var randomNum = UnityEngine.Random.Range(-5f, 5f);
            actions[i] = randomNum;
        }
    }

    private void FixedUpdate()
    {
        orientationObj.UpdateOrientation(hips, dodgeLookTarget.transform);
        var lookAtTargetReward = (Vector3.Dot(orientationObj.transform.forward, head.forward) + 1) * .5F;
        AddReward(lookAtTargetReward);

        var bullet = shooter.GetBullets();
        if (bullet != null)
        {
            //Debug.DrawRay(bullet.transform.position, bullet.Direction * 40f, Color.red);
        }
    }

    private void CollectObservationBodyPart(BodyPart bp, VectorSensor sensor)
    {
        //GROUND CHECK
        sensor.AddObservation(bp.groundContact.touchingGround); // Is this bp touching the ground

        sensor.AddObservation(orientationObj.transform.InverseTransformDirection(bp.rb.velocity));
        sensor.AddObservation(orientationObj.transform.InverseTransformDirection(bp.rb.angularVelocity));
        sensor.AddObservation(orientationObj.transform.InverseTransformDirection(bp.rb.position - hips.position));

        if (bp.rb.transform != hips)
        {
            sensor.AddObservation(bp.rb.transform.localRotation);
            sensor.AddObservation(bp.currentStrength / jdController.maxJointForceLimit);
        }
    }

    private Vector3 GetAvgVelocity()
    {
        var velSum = Vector3.zero;

        var numOfRb = 0;
        foreach (var item in jdController.bodyPartsList)
        {
            numOfRb++;
            velSum += item.rb.velocity;
        }

        var avgVel = velSum / numOfRb;
        return avgVel;
    }
}
