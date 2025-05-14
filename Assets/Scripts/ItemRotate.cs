using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    private float rotationSpeed = 50f;
    private float bobbingSpeed = 1f;
    private float bobbingAmount = 0.25f;

    private float yPosition;
    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
        // Set the original y position to the current y position
        yPosition = startPosition.y;
    }

    void Update()
    {
        // Rotate around the y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        
        // Bob up and down
        float newY = yPosition + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}