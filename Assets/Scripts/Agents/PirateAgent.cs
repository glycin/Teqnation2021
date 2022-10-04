using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PirateAgent : Agent
{
    [SerializeField]
    private Transform chest;

    [SerializeField]
    private Material normalMat;
    [SerializeField]
    private Material winMat;
    [SerializeField]
    private MeshRenderer rug;

    private float moveSpeed = 2.5f;
    private float startY;
    private float chestStartY;

    private void Start()
    {
        startY = transform.localPosition.y;
        chestStartY = chest.localPosition.y;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var moveX = actions.ContinuousActions[0];
        var moveZ = actions.ContinuousActions[1];

        transform.position += transform.forward * (moveX * moveSpeed * Time.deltaTime);
        transform.position += transform.right * (moveZ * moveSpeed * Time.deltaTime);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(chest.localPosition);
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-1.5f, 5f), startY, Random.Range(-10f, 9f));
        chest.localPosition = new Vector3(Random.Range(-2f, 5f), chestStartY, Random.Range(5f, 15f));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxisRaw("Vertical");
        actions[1] = Input.GetAxisRaw("Horizontal");
    }

    #region Animation Callbacks
    public void Shoot(int shooting)
    {
    }

    public void FootL(int step)
    {
    }

    public void FootR(int step)
    {
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            SetReward(1.0f);
            rug.material = winMat;
            EndEpisode();
        }

        if(other.tag == "Wall")
        {
            SetReward(-1f);
            rug.material = normalMat;
            EndEpisode();
        }
    }
}
