using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            float remainingTime = GameObject.Find("TimerText").GetComponent<TimerScript>().timeRemaining;
            GameObject.Find("CheckpointScript").GetComponent<CheckpointController>().CreateCheckpoint();
            GameObject.Find("ScoreController").GetComponent<ScoreScript>().AddScore(remainingTime);
        }
    }
}
