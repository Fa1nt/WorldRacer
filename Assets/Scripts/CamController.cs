using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    /*[SerializeField] private float translateSpeed;
    [SerializeField] private float rotationSpeed;*/
    private Transform target;
    [SerializeField] private float mouseSensitivity = 3.0f;
    private float rotationX;
    private float rotationY;
    [SerializeField] private float distanceFromTarget = 6.0f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX += mouseY;
        rotationY += mouseX;

        rotationX = Mathf.Clamp(rotationX, -30, 90);

        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);

        transform.position = target.TransformPoint(offset) - transform.forward * distanceFromTarget;
    }

    /*private void FixedUpdate()
    {
        HandleTranslation();
        HandleRotation();
    }

    private void HandleTranslation()
    {
        var targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }*/
}
