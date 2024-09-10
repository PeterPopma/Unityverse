using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neptune : MonoBehaviour
{
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
        float angle = (2 * Mathf.PI * Time.time * 0.00606796116504854368932038834951f * Settings.TimeScale) % (2 * Mathf.PI);
        transform.position = new Vector3(Mathf.Cos(angle) * Utilities.KmToWorldspaceUnits(4500000000), 0, Mathf.Sin(angle) * Utilities.KmToWorldspaceUnits(4500000000));

    }
}
