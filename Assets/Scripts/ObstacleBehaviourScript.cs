using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObstacleBehaviourScript : MonoBehaviour
{
    public float Force = 2;
    public bool AgentCollided = false;
    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody>();
        rigidBody.AddRelativeForce(Vector3.right * Force);   
    }
}
