using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraRotate : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 120f;
    public Vector2 pitchLimits = new Vector2(-30f, 60f);
    private float yaw;
    private float pitch;

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * rotationSpeed * Time.deltaTime;
        pitch -= mouseY * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        transform.position = target.position;
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
