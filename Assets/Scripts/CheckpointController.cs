using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public GameObject marker;

    public void CreateCheckpoint()
    {
        if (GameObject.FindGameObjectWithTag("Checkpoint") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Checkpoint"));
        }
        GameObject[] buildings;
        buildings = GameObject.FindGameObjectsWithTag("Building");
        int rand = Random.Range(0, buildings.Length);
        transform.position = buildings[rand].transform.position;
        transform.rotation = buildings[rand].transform.rotation;
        transform.Translate(Vector3.forward * (buildings[rand].transform.localScale.z / 2f + 10f));
        Instantiate(marker, transform.position, transform.rotation);
    }
}
