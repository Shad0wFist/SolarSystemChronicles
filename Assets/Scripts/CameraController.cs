using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public float maxAngle = 80.0f;
    private float rotationX = 0.0f;

    protected void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.parent.Rotate(Vector3.up * mouseX * sensitivity);

        rotationX -= mouseY * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -maxAngle, maxAngle);
        transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
    }
}


