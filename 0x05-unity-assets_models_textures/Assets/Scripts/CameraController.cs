using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    
    [Range(1,10)]
    public float rotateSpeed = 0.5f;
    private float mouseX;
    private float mouseY;
    private float cameraY;
    private float lastPlayerY;

    private Vector3 cameraOffset;

    void Start()
    {
        // Take camera offset based on initial position of player and camera
        cameraOffset = player.position - transform.position;

        cameraY = transform.position.y;
        lastPlayerY = player.position.y;
    }

    void LateUpdate()
    {
        mouseY = -Input.GetAxis("Mouse Y");

        cameraY += mouseY;

        // Quaternion cameraRotation = Quaternion.Euler(0f, mouseX, 0f);
        // cameraOffset = cameraRotation * cameraOffset;
        // transform.position = player.position - cameraOffset;

        // Constrain camera movement along Y axis 
        float playerY = player.position.y;
        cameraY = Mathf.Clamp(cameraY, playerY - 3, playerY + 7);

        // Combine mouse Y movement with tracking of player Y movement for smoother follow
        cameraY += (playerY - lastPlayerY) * 0.5f;
        // Store current player position for use in next frame 
        lastPlayerY = playerY;

        // float zMovement = (cameraY - playerY) * 0.5f;
        // Debug.Log(zMovement);

        // Adjust camera position along Y axis according to mouse/player movement
        transform.position = new Vector3(transform.position.x, cameraY, transform.position.z);

        // Rotate camera to always look at player
        transform.LookAt(player.position);
    }
}
