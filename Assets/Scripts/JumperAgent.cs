using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

using Unity.MLAgents;
using static UnityEngine.GraphicsBuffer;

public class JumperAgent : Agent
{
    public Transform Obstacle1;
    public Transform Obstacle2;
    public Transform Collectable1;
    public Transform Collectable2;


    public float speedMultiplier = 0.1f;
    public float rotationMultiplier = 5;
    public float jumpForce = 1000f;

    private bool targetPickedUpObstacle1 = false;
    private bool targetPickedUpObstacle2 = false;
    private bool targetPickedUpCollectable1 = false;
    private bool targetPickedUpCollectable2 = false;

    private bool isGrounded = true;

    public override void OnEpisodeBegin()
    {
        Obstacle1.gameObject.SetActive(true);
        Obstacle2.gameObject.SetActive(true);
        Collectable1.gameObject.SetActive(true);
        Collectable2.gameObject.SetActive(true);


        targetPickedUpObstacle1 = false;
        targetPickedUpObstacle2 = false;
        targetPickedUpCollectable1 = false;
        targetPickedUpCollectable2 = false;
        // reset de positie en orientatie als de agent gevallen is
     
        // verplaats de target naar een nieuwe willekeurige locatie 
        Obstacle1.localPosition = new Vector3(28, 1, 0);
        Obstacle2.localPosition = new Vector3(0, 1, -25);
        Collectable1.localPosition = new Vector3(24, 1, 0);
        Collectable2.localPosition = new Vector3(0, 1, -29);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent positie
        sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(TargetZone.localPosition);
        //sensor.AddObservation(Vector3.Distance(transform.localPosition, TargetZone.localPosition));
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Jumping action
        if (actionBuffers.DiscreteActions[0] == 1) // Assuming DiscreteActions[0] represents the jump action
        {
            Jump();
        }

        // Rewards
        //float distanceToTarget = Vector3.Distance(transform.localPosition, Target.localPosition);

        // Target reached
        //if (!targetPickedUp && distanceToTarget < 1.42f)
        //{
        //    Target.gameObject.SetActive(false);
        //    targetPickedUp = true;
        //    AddReward(0.4f);
        //}

        //// Back at TargetZone
        //if (targetPickedUp && transform.localPosition.x < -5)
        //{
        //    AddReward(0.6f);
        //    EndEpisode();
        //}

        //// Fallen off the platform?
        //if (transform.localPosition.y < 0)
        //{
        //    SetReward(0);
        //    EndEpisode();
        //}
    }

    void Jump()
    {
        if (GetComponent<Rigidbody>() != null) // Assuming you have a way to check if the object is on the ground
        {
            // Apply a jump force in the upward direction
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}
