using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject vehicle;
    public GameObject followCamera;
    public GameObject otherCamera;
    public GameObject mapCamera;
    public GameObject uiElement1;
    public GameObject uiElement2;
    public GameObject uiElement3;
    public GameObject uiElement4;
    public GameObject uiElement5;
    public GameObject uiElement6;
    public GameObject pauseScript;
    public GameObject miniMap;

    public void SpawnVehicle()
    {
        Vector3 pos = GameObject.FindGameObjectWithTag("Road").transform.position;
        pos.y = 5;
        Instantiate(vehicle, pos, Quaternion.identity);
        followCamera.SetActive(true);
        otherCamera.SetActive(false);
        mapCamera.SetActive(true);

        uiElement1.SetActive(false);
        uiElement2.SetActive(false);
        uiElement3.SetActive(false);
        uiElement4.SetActive(false);
        uiElement5.SetActive(false);
        uiElement6.SetActive(false);
        pauseScript.SetActive(true);
        miniMap.SetActive(true);

        Cursor.visible = false;
    }
}
