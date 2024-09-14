using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utilities
{
    public const float KM_PER_WORLDSPACE_UNIT = 1000000f; //2785400f;

    public static float KmToWorldspaceUnits(float km)
    {
        return km / KM_PER_WORLDSPACE_UNIT;
    }

    public static float WorldspaceUnitsToKM(float units)
    {
        return units * KM_PER_WORLDSPACE_UNIT;
    }
    
    public static Transform[] FindChildren(this Transform transform, string name)
    {
        return transform.GetComponentsInChildren<Transform>().Where(t => t.name == name).ToArray();
    }
}
