using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class QuidditchAgent : Agent
{
    [SerializeField]
    private Transform snitch;

    private float flySpeed = 9f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float previousStepDistance;
    private float currentStepDistance;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = transform.parent.GetComponent<ScoreManager>();
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        previousStepDistance = Vector3.Distance(transform.position, snitch.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var moveX = actions.ContinuousActions[0];
        var moveZ = actions.ContinuousActions[1];
        var moveY = actions.ContinuousActions[2];

        transform.localPosition += transform.forward * (moveX * flySpeed * Time.deltaTime);
        transform.localPosition += transform.right * (moveZ * flySpeed * Time.deltaTime);
        transform.localPosition += transform.up * (moveY * flySpeed * Time.deltaTime);
        transform.LookAt(snitch.transform);
        if(currentStepDistance < previousStepDistance)
        {
            SetReward(0.01f);
        }

        previousStepDistance = currentStepDistance;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(snitch.localPosition);
        currentStepDistance = Vector3.Distance(transform.localPosition, snitch.localPosition);
        sensor.AddObservation(currentStepDistance);
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;

        var moveX = Input.GetAxisRaw("Vertical");
        var moveZ = Input.GetAxisRaw("Horizontal");
        var moveY = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            moveY = 1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            moveY = -1f;
        }

        actions[0] = moveX;
        actions[1] = moveZ;
        actions[2] = moveY;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            SetReward(-1f);
            EndEpisode();
        }

        SetReward(-0.1f);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Goal")
        {
            if(transform.name == "Witch")
            {
                scoreManager.AddToLeft();
            }
            else
            {
                scoreManager.AddToRight();
            }
            SetReward(1f);
            collision.collider.GetComponent<SnitchAgent>().End();
            EndEpisode();
        }
    }
}
