using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMarkings : MonoBehaviour
{
    private void Start()
    {
        int roads = 0;
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f, transform.rotation);
        for (int j = 0; j < colliders.Length; j++)
        {
            if (colliders[j].tag == "Road")
            {
                roads++;
            }
        }
        if (roads >= 2)
        {
            Destroy(transform.gameObject);
        }
    }
}
