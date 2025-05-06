using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform[] spawnPoints;
    public RequestData[] requestPool;
    public float spawnInterval = 5f;

    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <=0f)
        {
            SpawnCustomer();
            timer = spawnInterval;
        }
    }

    void SpawnCustomer()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);

        Customer customer = newCustomer.GetComponent<Customer>();
        customer.requestData = requestPool[Random.Range(0, requestPool.Length)];
    }
}
