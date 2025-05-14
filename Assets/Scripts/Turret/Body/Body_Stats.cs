using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body_Stats : MonoBehaviour
{
    public float speed;
    public int price;

    public Transform target;

    public float detectionRadius;

    void Start()
    {
        speed = 4f;
        detectionRadius = 30f;
    }

    public Body_Stats(float speed)
    {
        this.speed = speed;
    }
}
