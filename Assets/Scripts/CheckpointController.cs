using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    AudioSource audioSource;
    public GameObject marker;
    public GameObject mapPoint;
    private GameObject pointDot;
    private GameObject checkPoint;
    public GameObject timer;
    public GameObject score;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void CreateCheckpoint()
    {
        if (GameObject.FindGameObjectWithTag("Checkpoint") != null)
        {
            audioSource.Play();
            Destroy(GameObject.FindGameObjectWithTag("Checkpoint"));
            Destroy(pointDot);
        }
        GameObject[] buildings;
        buildings = GameObject.FindGameObjectsWithTag("Building");
        int rand = Random.Range(0, buildings.Length);
        transform.position = buildings[rand].transform.position;
        transform.rotation = buildings[rand].transform.rotation;
        transform.Translate(Vector3.forward * (buildings[rand].transform.localScale.z / 2f + 10f));
        transform.Translate(Vector3.down * (buildings[rand].transform.position.y));
        checkPoint = Instantiate(marker, transform.position, transform.rotation);

        pointDot = Instantiate(mapPoint);
        pointDot.transform.position = transform.position;

        GameObject car = GameObject.FindGameObjectWithTag("Player");
        float dist = Vector3.Distance(checkPoint.transform.position, car.transform.position);
        //Debug.Log(string.Format("Distance between {0} and {1} is: {2}", checkPoint, car, dist));

        timer.GetComponent<TimerScript>().timerIsRunning = true;
        timer.GetComponent<TimerScript>().timeRemaining = dist/10 + 5;
        // check the distance between the player and the checkpoint and give a timeRemaining based on that
        score.GetComponent<ScoreScript>().reward = (int) (dist / 10 / 4);
    }
}
