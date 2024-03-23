using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        ApplyFlockRules();
        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }

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
                vcentre = vcentre / groupSize;
                gSpeed = gSpeed / groupSize;

                // set local speed to the average speed of the group but capped to FM max speed
                speed = gSpeed > FlockManager.FM.maxSpeed ? FlockManager.FM.maxSpeed : gSpeed;

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
