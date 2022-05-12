using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sidewalks : MonoBehaviour
{
    private void Start()
    {
        Vector3 scaleChange = new Vector3(0.0f, 0.0f, -0.1f);
        Vector3 posChange = new Vector3(0.0f, 0.0f, -0.15f);

        GameObject empty = new GameObject();
        empty.transform.position = transform.position;
        empty.transform.localScale = transform.localScale;
        empty.transform.rotation = transform.rotation;

        Transform overlap = empty.transform;
        overlap.transform.localScale += scaleChange;
        overlap.transform.position += posChange;
        Collider[] colliders = Physics.OverlapBox(overlap.transform.position, overlap.transform.localScale, overlap.transform.rotation);
        int col = 0;
        for (int j = 0; j < colliders.Length; j++)
        {
            if (colliders[j].tag == "Road")
            {
                col++;
            }
        }
        Destroy(empty);
        if (col > 3)
        {
            Destroy(transform.gameObject);
        }
        else if (col > 2)
        {
            scaleChange = new Vector3(-0.3f, 0.0f, 0.0f);
            transform.localScale += scaleChange;
        }
    }
}
