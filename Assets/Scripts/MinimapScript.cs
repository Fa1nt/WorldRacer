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
        var planeRenderer = duplicatePlane.GetComponent<Renderer>();
        planeRenderer.material.SetColor("_Color", Color.gray);

        foreach (GameObject road in roads)
        {
            GameObject duplicate = Instantiate(road, road.transform.position, road.transform.rotation);
            duplicate.layer = LayerMinimap;
            var cubeRenderer = duplicate.GetComponent<Renderer>();
            cubeRenderer.material.SetColor("_Color", Color.black);
        }
    }

    void CheckPoint()
    {
        if (GameObject.Find("Map Checkpoint(Clone)") != null)
        {
            GameObject mapPoint = GameObject.Find("Map Checkpoint(Clone)");
            GameObject checkPoint = GameObject.FindGameObjectWithTag("Checkpoint");
            GameObject car = GameObject.FindGameObjectWithTag("Player");
            float dist = Vector3.Distance(checkPoint.transform.position, car.transform.position);
            //Debug.Log(string.Format("Distance between {0} and {1} is: {2}", checkPoint, car, dist));
            if (dist > 175f)
            {
                mapPoint.transform.position = car.transform.position;
                mapPoint.transform.LookAt(checkPoint.transform);
                mapPoint.transform.Translate(Vector3.forward * 175f);
            }
            else
            {
                mapPoint.transform.position = checkPoint.transform.position;
            }
        }
    }

    void LateUpdate()
    {
        Vector3 newPosition = target.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        newPosition.x = transform.position.x;
        newPosition.y = 0.5f;
        newPosition.z = transform.position.z;
        playerDot.transform.position = newPosition;

        Vector3 newAngles = target.rotation.eulerAngles;
        newAngles.x = newAngles.z = 0;
        playerDot.transform.eulerAngles = newAngles;

        // rotate minimap
        rotationY = GameObject.Find("Car Camera").transform.localRotation.eulerAngles.y;
        transform.localEulerAngles = new Vector3(90, rotationY, 0);
        CheckPoint();
    }
}
