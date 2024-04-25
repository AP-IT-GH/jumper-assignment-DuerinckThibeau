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

    public float jumpForce = 20;

    private bool isGrounded = true;

    private float timeElapsed = 0;
    private float episodeDuration = 11f;

    void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.tag == "Obstacle")
      {
        AddReward(-0.2f);
        EndEpisode();
      }
      if(other.gameObject.tag == "Collectable")
      {
        AddReward(0.2f);
        other.gameObject.SetActive(false);
      }
    }


    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
          isGrounded = true;
        }
    }

    public override void OnEpisodeBegin()
    {
        Obstacle1.gameObject.SetActive(true);
        Obstacle2.gameObject.SetActive(true);
        Collectable1.gameObject.SetActive(true);
        Collectable2.gameObject.SetActive(true);

        Obstacle1.localPosition = new Vector3(14, 1, 0);
        Obstacle2.localPosition = new Vector3(0, 1, -70);
        Collectable1.localPosition = new Vector3(30, 1, 0);
        Collectable2.localPosition = new Vector3(0, 1, -55);
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
        else if (transform.localPosition.y < 0)
        {
            AddReward(-0.4f);
            EndEpisode();
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
          transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * jumpForce);
          isGrounded = false;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    public  void Update()
    {
      timeElapsed += Time.fixedDeltaTime;  

      if (timeElapsed >= episodeDuration)  
      {
        AddReward(0.5f);
        timeElapsed = 0;
        EndEpisode();  
      }

    }
}
