using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGameManager : MonoBehaviour
{
    public static StageGameManager Instance;
    [Header("�մ� ����")]
    public List<Customer> customers; // �����Ϳ��� 5�� �ֱ�

    [Header("��û ����")]
    public int maxActiveRequests = 3;

    [Header("������")]
    public float satisfaction = 100f;

    [Header("��û ����")]
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
            // TODO: ���ӿ��� ó��
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
            case 1: return findBookRequest; // �̰� ������ borrow �����̿�����
            case 2: return quietDownRequest;
            default: return borrowBookRequest;
        }
    }


}
