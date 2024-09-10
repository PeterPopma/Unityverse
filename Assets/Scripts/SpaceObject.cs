using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    const float MINIMUM_OBJECT_SIZE = 0.02f;

    [SerializeField] float radius;                  // radius in km
    [SerializeField] float meshRadius;              // radius of the planets mesh in worldspace units
    [SerializeField] float relativeSize;            // the relative size of the object at large distance (e.g. the sun should appear 2 times the size of earth)
    float minimumViewDistance;                      // minium view distance in worldspace units
    float defaultScaleInWorldSpace;

    public float Radius { get => radius; set => radius = value; }
    public float MinimumViewDistance { get => minimumViewDistance; set => minimumViewDistance = value; }
    public float MeshRadius { get => meshRadius; set => meshRadius = value; }

    public void Start()
    {
        defaultScaleInWorldSpace = Utilities.KmToWorldspaceUnits(radius) / meshRadius;
        minimumViewDistance = Utilities.KmToWorldspaceUnits(radius) * 3;
        while (defaultScaleInWorldSpace < MINIMUM_OBJECT_SIZE)
        {
            defaultScaleInWorldSpace *= 2;
            minimumViewDistance *= 2;
        }
    }

    // the object should be scaled relative to the camera distance and the size it should appear.
    // only when very close, we can use the real radius in world space.
    //
    // at camera distance 0.4 the earth is visible at scale 1
    private void Scale()
    {
        float distancefromCamera = (transform.position - Game.Instance.VMCamera.transform.position).magnitude;
        float scaleForFixedSize = MINIMUM_OBJECT_SIZE * distancefromCamera / meshRadius * relativeSize;

        if (defaultScaleInWorldSpace < scaleForFixedSize)
        {
            // show the object at a constant size
            transform.localScale = new Vector3(scaleForFixedSize, scaleForFixedSize, scaleForFixedSize);
        }
        else
        {
            // show the object at real size
            transform.localScale = new Vector3(defaultScaleInWorldSpace, defaultScaleInWorldSpace, defaultScaleInWorldSpace);
        }
    }

    public void Update()
    {
        Scale();
    }
}
