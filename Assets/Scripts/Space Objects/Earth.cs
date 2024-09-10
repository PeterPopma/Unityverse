using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
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
        // in one year the earth rotates 360 * 365 = 131400 degrees
        transform.Find("Body").Rotate(new Vector3(0, -Settings.TimeScale * 131400 * Time.deltaTime, 0), Space.Self);

        float angle = (2 * Mathf.PI * Time.time * Settings.TimeScale) % (2 * Mathf.PI);
        transform.position = new Vector3(Mathf.Cos(angle) * Utilities.KmToWorldspaceUnits(149600000), 0, Mathf.Sin(angle) * Utilities.KmToWorldspaceUnits(149600000));
    }
}
