using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGameManager : MonoBehaviour
{
    public static StageGameManager Instance;
    [Header("손님 관련")]
    public List<Customer> customers; // 에디터에서 5명 넣기

    [Header("요청 제한")]
    public int maxActiveRequests = 3;

    [Header("만족도")]
    public float satisfaction = 100f;

    [Header("요청 종류")]
    public RequestData borrowBookRequest;
    public RequestData quietDownRequest;
    public RequestData readingRequest;
    public RequestData returnBookRequest;
    public RequestData readyRequest;
    public RequestData findBookRequest;

    private int currentActiveRequestCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        foreach (Customer customer in customers)
        {
            StartCoroutine(RequestRoutine(customer));
        }
    }

    public void DecreaseSatisfaction(float amount)
    {
        satisfaction -= amount;
        if (satisfaction <= 0)
        {
            satisfaction = 0;
            Debug.Log("Game Over!");
            // TODO: 게임오버 처리
        }
    }

    public void OnRequestStarted()
    {
        currentActiveRequestCount++;
    }

    public void OnRequestEnded()
    {
        currentActiveRequestCount--;
    }

    private IEnumerator RequestRoutine(Customer customer)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10f, 30f));

            if (!customer.isRequestActive && currentActiveRequestCount < maxActiveRequests)
            {
                RequestData chosen = PickRandomRequest();
                customer.AssignRequest(chosen);
                OnRequestStarted();
            }
        }
    }

    private RequestData PickRandomRequest()
    {
        int roll = Random.Range(0, 3);
        switch (roll)
        {
            case 0: return borrowBookRequest;
            case 1: return findBookRequest; // 이건 무조건 borrow 다음이여야해
            case 2: return quietDownRequest;
            default: return borrowBookRequest;
        }
    }


}
