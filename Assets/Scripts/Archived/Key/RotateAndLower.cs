using UnityEngine;

public class RotateAndLower : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float lowerSpeed = 1f;
    [SerializeField] float targetYPosition = 0f;
    [SerializeField] float bobbingSpeed = 1f;
    [SerializeField] float bobbingAmount = 0.5f;

    private bool reachedTarget = false;
    private float originalY;

    void Start()
    {
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate around the y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Lower on the y-axis until it reaches the target
        if (!reachedTarget)
        {
            if (transform.position.y > targetYPosition)
            {
                float newY = Mathf.Max(transform.position.y - lowerSpeed * Time.deltaTime, targetYPosition);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
            else
            {
                reachedTarget = true;
                originalY = targetYPosition;
            }
        }
        else
        {
            // Bob up and down
            float newY = originalY + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
