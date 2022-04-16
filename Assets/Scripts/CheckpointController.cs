using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public GameObject marker;
    public GameObject mapPoint;
    private GameObject pointDot;

    public void CreateCheckpoint()
    {
        if (GameObject.FindGameObjectWithTag("Checkpoint") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Checkpoint"));
            Destroy(pointDot);
        }
        GameObject[] buildings;
        buildings = GameObject.FindGameObjectsWithTag("Building");
        int rand = Random.Range(0, buildings.Length);
        transform.position = buildings[rand].transform.position;
        transform.rotation = buildings[rand].transform.rotation;
        transform.Translate(Vector3.forward * (buildings[rand].transform.localScale.z / 2f + 10f));
        Instantiate(marker, transform.position, transform.rotation);

        pointDot = Instantiate(mapPoint);
        pointDot.transform.position = transform.position;
    }
}
