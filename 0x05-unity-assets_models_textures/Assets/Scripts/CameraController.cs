using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    
    [Range(1,10)]
    public float rotateSpeed = 5.0f;

    private Vector3 cameraOffset;

    void Start()
    {
        // Take camera offset based on initial position of player and camera
        cameraOffset = player.position - transform.position;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * -1;

        Quaternion cameraRotation = Quaternion.Euler(mouseY, mouseX, 0f);
        cameraOffset = cameraRotation * cameraOffset;

        transform.position = player.position - cameraOffset;
        transform.position = new Vector3(transform.position.x,
            Mathf.Clamp(transform.position.y, player.position.y - 1, player.position.y + 3),
            transform.position.z);
        transform.LookAt(player.position);
    }
}
