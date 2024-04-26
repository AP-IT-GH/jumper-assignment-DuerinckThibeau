using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using static UnityEngine.GraphicsBuffer;

public class AgentBehaviour : Agent
{
    public float JumpForce = 20;

    private bool canJump = true;

    public ObstacleCounterScript counterScript;

    void OnCollisionEnter(Collision collision)
    {
        ObstacleBehaviourScript obstacleBehaviourScript = collision.gameObject.GetComponent<ObstacleBehaviourScript>();
        if (collision.gameObject.tag == "Obstacle" && obstacleBehaviourScript.AgentCollided == false )
        {
            AddReward(-0.2f);
            obstacleBehaviourScript.AgentCollided = true;
        }

        if (collision.gameObject.tag == "Collectable" && obstacleBehaviourScript.AgentCollided == false)
        {
            AddReward(0.5f);
            Destroy(collision.gameObject);
            counterScript.PassedObstacles++;
            obstacleBehaviourScript.AgentCollided = true;
        }

        if (collision.gameObject.tag == "Floor")
        {
            canJump = true;
        }
    }
    
    void Jump()
    {
        if(canJump)
        {
            transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * JumpForce);
            canJump = false;
            AddReward(-0.10f);
        }
    }

    public override void OnEpisodeBegin()
    {
        SetReward(1.25f);
        transform.localPosition = new Vector3(0, 0.5f, 0);
        transform.localEulerAngles = new Vector3(0, -90, 0);

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach(GameObject obstacle in obstacles) {
            Destroy(obstacle);
        }

        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (GameObject collectable in collectables)
        {
            Destroy(collectable);
        }

        ObstacleCounterScript.ResetInstance();
        ObstacleSpawnerScript.ResetInstance();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (actionBuffers.DiscreteActions[0] == 1)
        {
            Jump();
        }

        if (ObstacleCounterScript.GetInstance().PassedObstacles >= 5)
        {
            EndEpisode();
        }
        else if (transform.localPosition.y < 0)
        {
            AddReward(-0.4f);
            EndEpisode();
        }
    }
}
