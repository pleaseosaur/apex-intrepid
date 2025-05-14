using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float distanceFromPlayer = 5f;
    [SerializeField] private Vector3 lookAtOffset = new Vector3(0, 2, 0);
    private float horizontalAngle = 180f;
    private float verticalAngle = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;                                    //  Lock the cursor.
        Cursor.visible = false;                                                      //  Hide it.
        
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        Rotate();
        transform.LookAt(player.position + lookAtOffset);                            //  Ensure camera is always looking at the player.
    }

/// <summary>
/// This method tracks the mouse to adjust camera angle and player rotation.
/// </summary>
    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        horizontalAngle += mouseX;
        verticalAngle -= mouseY;
        verticalAngle = Mathf.Clamp(verticalAngle, -10f, 60f);                       //  Keep camera within bounds.

        player.Rotate(Vector3.up * mouseX);                                          //  Rotate the player.

        float x = distanceFromPlayer * Mathf.Cos(verticalAngle * Mathf.Deg2Rad) * Mathf.Sin(horizontalAngle * Mathf.Deg2Rad);
        float y = distanceFromPlayer * Mathf.Sin(verticalAngle * Mathf.Deg2Rad);
        float z = distanceFromPlayer * Mathf.Cos(verticalAngle * Mathf.Deg2Rad) * Mathf.Cos(horizontalAngle * Mathf.Deg2Rad);

        transform.position = player.position + lookAtOffset + new Vector3(x, y, z);
    }
}
