using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCounterScript : MonoBehaviour
{
    public int PassedObstacles = 0;

    private static ObstacleCounterScript instance = new ObstacleCounterScript();
    public static ObstacleCounterScript GetInstance()
    {
        return instance;
    }

    public static void ResetInstance()
    {
        instance.PassedObstacles = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Obstacle" && other.gameObject.tag == "Collectable")
        {
            instance.PassedObstacles++;
            Destroy(other.gameObject);
        }
    }
}
