using UnityEngine;

public class MoveLightning : MonoBehaviour
{
    private float speed = 3.0f;
    private float maxSpeed = 11.0f;
    private float currentSpeed = 0.0f;
    public GameObject lightning;
    public GameObject startPoint;
    public GameObject endPoint;
    private float minHeight = 0.1f;
    private float maxHeight = 1.4f;

    private Vector3 targetPosition;

    void Start()
    {
        ResetLightning();
    }

    void Update()
    {
        // Move the lightning towards the target position
        float currentSpeed = Random.Range(speed, maxSpeed);
        lightning.transform.position = Vector3.MoveTowards(lightning.transform.position, targetPosition, currentSpeed * Time.deltaTime);

        // If the lightning is very close to the endPoint, reset it
        if (Vector3.Distance(lightning.transform.position, endPoint.transform.position) < 0.01f)
        {
            ResetLightning();
        }
    }

    void ResetLightning()
    {
        // Get the start point's position
        Vector3 newStartPosition = startPoint.transform.position;
        Vector3 newEndPosition = endPoint.transform.position;
        
        // Randomize the Y component within the specified range
        newStartPosition.y = Random.Range(minHeight, maxHeight);
        newEndPosition.y = Random.Range(minHeight, maxHeight);
        
        // Set the lightning's position to the new randomized start position
        Delay();
        lightning.transform.position = newStartPosition;
        
        // Set the target to the end point's position
        targetPosition = endPoint.transform.position;
    }
    
    void Delay()
    {
        // random wait between 0.5 and 2 seconds
        float delay = Random.Range(0.5f, 2.0f);
        Invoke("ResetLightning", delay);
    }
}