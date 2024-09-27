using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    private GameObject destination1;
    private GameObject destination2;
    private GameObject currentDestination;
    private bool reachedDestination;
    private float speed = 16900;        // in m/s
    private decimal currentDestinationDistance;

    public GameObject Destination1 { get => destination1; set => destination1 = value; }
    public GameObject Destination2 { get => destination2; set => destination2 = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool ReachedDestination { get => reachedDestination; set => reachedDestination = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Game.Instance.Playing)
        {
            return;
        }

        if (currentDestination != null)
        {
            Vector3 direction = currentDestination.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
            if (reachedDestination)
            {
                // circle destination object at 3 times its radius
                float radius = 3 * currentDestination.GetComponent<SpaceObject>().Radius / Utilities.KM_PER_WORLDSPACE_UNIT;
                float kmTravelled = 365 * 24 * 60 * 60 * Settings.TimeScale * Time.time * Speed;
                float currentAngle = kmTravelled / (2 * Mathf.PI * currentDestination.GetComponent<SpaceObject>().Radius);
                transform.position = currentDestination.transform.position + new Vector3(radius * Mathf.Sin(currentAngle), 0, radius * Mathf.Cos(currentAngle));
            }
            else
            {
                float distanceToDestination = direction.magnitude - currentDestination.GetComponent<SpaceObject>().MeshRadius * 2;
                float worldSpaceUnitsToTravel = 365 * 24 * 60 * 60 * Settings.TimeScale * Time.deltaTime * Speed / Utilities.KM_PER_WORLDSPACE_UNIT;

                if (worldSpaceUnitsToTravel > distanceToDestination)
                {
                    worldSpaceUnitsToTravel = distanceToDestination;
                    NextSpaceshipDestination();
                }

                // move to the new location
                transform.position += direction.normalized * worldSpaceUnitsToTravel;
            }
        }
    }

    public void NextSpaceshipDestination()
    {
        reachedDestination = true;
        if (Destination1 != Destination2)
        {
            if (currentDestination == Destination1 && Destination2 != null)
            {
                currentDestination = Destination2;
                reachedDestination = false;
            }
            else
            {
                if (currentDestination != Destination1)
                {
                    reachedDestination = false;
                }
                currentDestination = Destination1;
            }
            currentDestinationDistance = SquaredMagnitude(currentDestination.transform.position, transform.position);
        }
    }

    // Unity gives us only float, but we need higher precision
    private decimal SquaredMagnitude(Vector3 point1, Vector3 point2)
    {
        return ((decimal)point1.x-(decimal)point2.x) * ((decimal)point1.x - (decimal)point2.x) +
            ((decimal)point1.y - (decimal)point2.y) * ((decimal)point1.y - (decimal)point2.y) +
            ((decimal)point1.z - (decimal)point2.z) * ((decimal)point1.z - (decimal)point2.z);
    }

    public string GetProgress()
    {
        if (currentDestination == null)
            return "";

        decimal currentDistance = SquaredMagnitude(currentDestination.transform.position, transform.position);
        decimal progress = (1 - (currentDistance / currentDestinationDistance))*100;
        return progress.ToString("0.00000000000000000000");
    }
}
