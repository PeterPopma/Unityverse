using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pluto : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!Game.Instance.Playing)
        {
            return;
        }
        float angle = (2 * Mathf.PI * Time.time * 0.00403225806451612903225806451613f * Settings.TimeScale) % (2 * Mathf.PI);
        transform.position = new Vector3(Mathf.Cos(angle) * Utilities.KmToWorldspaceUnits(5900000000), 0, Mathf.Sin(angle) * Utilities.KmToWorldspaceUnits(5900000000));

    }
}
