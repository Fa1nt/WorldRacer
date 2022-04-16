using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    private Transform target;
    private float rotationY;
    public GameObject mapPlayer;
    private GameObject playerDot;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        playerDot = Instantiate(mapPlayer);

        GameObject plane = GameObject.Find("Plane");
        GameObject[] roads;
        roads = GameObject.FindGameObjectsWithTag("Road");

        int LayerMinimap = LayerMask.NameToLayer("Minimap");
        GameObject duplicatePlane = Instantiate(plane, plane.transform.position, plane.transform.rotation);
        duplicatePlane.layer = LayerMinimap;

        foreach (GameObject road in roads)
        {
            GameObject duplicate = Instantiate(road, road.transform.position, road.transform.rotation);
            duplicate.layer = LayerMinimap;
            var cubeRenderer = duplicate.GetComponent<Renderer>();
            cubeRenderer.material.SetColor("_Color", Color.black);
        }
    }

    void LateUpdate()
    {
        Vector3 newPosition = target.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        newPosition.x = transform.position.x;
        newPosition.y = 0;
        newPosition.z = transform.position.z;
        playerDot.transform.position = newPosition;

        rotationY = GameObject.Find("Car Camera").transform.localRotation.eulerAngles.y;
        transform.localEulerAngles = new Vector3(90, rotationY, 0);
    }
}
