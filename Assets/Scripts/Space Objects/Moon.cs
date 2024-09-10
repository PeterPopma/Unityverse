using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    [SerializeField] GameObject earth;

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
        // moon rotates 12 times a year around the earth
        float angle = (2 * Mathf.PI * Time.time * Settings.TimeScale * 12) % (2 * Mathf.PI);
        float radius = Utilities.KmToWorldspaceUnits(384400);
        transform.position = new Vector3(earth.transform.position.x + Mathf.Sin(angle) * Utilities.KmToWorldspaceUnits(384400), earth.transform.position.y, earth.transform.position.z + Mathf.Cos(angle) * Utilities.KmToWorldspaceUnits(384400));

        // moon rotates with the same speed around its axis as it rotates around earth
        transform.rotation = Quaternion.Euler(120, angle * 180 / Mathf.PI, 0);
    }
}
