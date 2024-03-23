using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public static FlockManager FM; // Singleton instance of FlockManager
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(5, 5, 5);

    public Vector3 goalPos;

    [Header("Fish Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.05f, 0.2f)]
    public float rotationSpeed;
    [Range(0.5f, 2.0f)]
    public float avoidDistance;

    [Range(1, 1000)]
    public float changeGoalPosRate;
    [Range(1, 50)]
    public float changeSpeedRate;

    // Start is called before the first frame update
    void Start()
    {
        allFish = new GameObject[numFish];
        for (int i = 0; i < numFish; i++)
        {
            // Assign random position to fish relative to this (FlockManager) object
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));

            allFish[i] = Instantiate(fishPrefab, pos, Quaternion.identity);

        }

        // Set the FlockManager instance to this object
        FM = this;
        goalPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the goal position of the flock with a certain probability
        if (Random.Range(0, changeGoalPosRate) <= 1)
        {
            // the new goalPos will still be within swimLimits, we are not updating position of this GO!
            goalPos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                Random.Range(-swimLimits.z, swimLimits.z));
        }
    }
}
