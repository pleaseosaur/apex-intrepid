using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timed_Spawner : MonoBehaviour
{
    [SerializeField] GameObject key;
    [SerializeField] Vector3 keyLocation;
    [SerializeField] float keyDelay;

    // Start is called before the first frame update
    void Start()
    {
        // Invoke("SpawnKey", keyDelay); // key spawning disabled to test drops
    }

    private void SpawnKey()
    {
        if (key != null)
        {
            Instantiate(key, keyLocation, Quaternion.identity);
        }
    }
}
