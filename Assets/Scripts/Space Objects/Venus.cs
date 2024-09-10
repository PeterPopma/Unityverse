using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venus : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        if (!Game.Instance.Playing)
        {
            return;
        }
        float angle = (2 * Mathf.PI * Time.time * 1.6233616133511111111111111111111f * Settings.TimeScale) % (2 * Mathf.PI);
        transform.position = new Vector3(Mathf.Cos(angle) * Utilities.KmToWorldspaceUnits(108200000), 0, Mathf.Sin(angle) * Utilities.KmToWorldspaceUnits(108200000));

    }
}
