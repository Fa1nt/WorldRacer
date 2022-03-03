﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject vehicle;
    public GameObject followCamera;
    public GameObject otherCamera;

    public void SpawnVehicle()
    {
        //Vector3 pos = new Vector3(0, 0, 0);
        Vector3 pos = GameObject.FindGameObjectWithTag("Node").transform.position;
        pos.y = 5;
        Instantiate(vehicle, pos, Quaternion.identity);
        followCamera.SetActive(true);
        otherCamera.SetActive(false);
    }
}