using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    float speed;
    bool turning = false;
    Bounds b;


    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);

        // set up a bounds b, e.g. a prism twice the size of swimLimits centered on the FlockManager
        b = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.swimLimits * 2);
    }

    // Update is called once per frame
    void Update()
    {
        // check if this fish is in bounds of b, a prism twice the size of swimLimits. if no, set turning to true
        if (!b.Contains(this.transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }   

        // if turning, adjust to return to centre of flock position
        if (turning)
        {
            ReturnToFlock();
        }
        else
        {
            // change speed with a certain probability
            if (Random.Range(0, FlockManager.FM.changeSpeedRate) <= 1)
            {
                speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
            }

            // Apply flocking rules with a certain probability to reduce load
            if (Random.Range(0, 100) <= 10)
            {
                ApplyFlockRules();
            }
        }

        // move forward
        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }

    
    /// <summary>
    /// Sets destination of this fish back to the center of the flock by adjusting its rotation.
    /// </summary>
    void ReturnToFlock()
    {
        Vector3 direction = FlockManager.FM.transform.position - this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                   Quaternion.LookRotation(direction),
                                                   FlockManager.FM.rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Apply three rules of flocking: cohesion, separation and alignment.
    /// </summary>
    void ApplyFlockRules()
    {
        GameObject[] gos;
        gos = FlockManager.FM.allFish;

        // set up vectors and counters for the rules
        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        float groupSize = 0;

        // loop through all the fish and check for distance against any other fish which is not this GO
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);

                // if neighbouring fish is within the distance, use its position and heading to calculate the flocking rules
                if (nDistance <= FlockManager.FM.neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;
                }

                // if too close, avoid neighbouring fish and influence vavoid vector
                if (nDistance < FlockManager.FM.avoidDistance)
                {
                    vavoid = vavoid + (this.transform.position - go.transform.position); // opposite direction to neighbouring fish
                }

                // find speed of neighbouring fish and use to adjust speed of this fish
                Flock anotherFlock = go.GetComponent<Flock>();
                gSpeed += anotherFlock.speed;


            }

            // take average of vcentre, vavoid and flock speed if groupsize is greater than 0
            if (groupSize > 0)
            {
                // blend in a goal position to the vcentre vector
                vcentre = vcentre / groupSize + (FlockManager.FM.goalPos - this.transform.position);
                gSpeed = gSpeed / groupSize;

                // set local speed to the average speed of the group but capped to FM max speed
                speed = Mathf.Clamp(gSpeed, FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);

                // calculate the direction to move towards the center of the group
                Vector3 direction = (vcentre + vavoid) - this.transform.position;

                // slerp to the direction of the center of the group
                if (direction != Vector3.zero)
                {
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                               Quaternion.LookRotation(direction),
                                                               FlockManager.FM.rotationSpeed * Time.deltaTime);
                }
            }

        }

    }
}
