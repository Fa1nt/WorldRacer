using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.Find("CheckpointScript").GetComponent<CheckpointController>().CreateCheckpoint();
        }
    }
}
