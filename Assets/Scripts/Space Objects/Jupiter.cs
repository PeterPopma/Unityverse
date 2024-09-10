using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jupiter : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (!Game.Instance.Playing)
        {
            return;
        }
        float angle = (2 * Mathf.PI * Time.time / 11.862f * Settings.TimeScale) % (2 * Mathf.PI);
        transform.position = new Vector3(Mathf.Cos(angle) * Utilities.KmToWorldspaceUnits(741340000), 0, Mathf.Sin(angle) * Utilities.KmToWorldspaceUnits(741340000));
    }
}
