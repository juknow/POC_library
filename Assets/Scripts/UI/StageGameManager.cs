using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageGameManager : MonoBehaviour
{
    public static StageGameManager Instance;
    [Header("�մ� ����")]
    public List<Customer> customers; // �����Ϳ��� 5�� �ֱ�

    [Header("��û ����")]
    public int maxActiveRequests = 3;

    [Header("������")]
    public float satisfaction = 100f;
    public TextMeshProUGUI satisText;
    public TextMeshProUGUI gameOverText;


    [Header("��û ����")]
    public RequestData borrowBookRequest;
    public RequestData quietDownRequest;
    public RequestData readingRequest;
    public RequestData returnBookRequest;
    public RequestData readyRequest;
    public RequestData findBookRequest;

    [Header("����ǰUI")]
    public TextMeshProUGUI inventoryText;

    public int currentActiveRequestCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        gameOverText.gameObject.SetActive(false);
        UpdateSatisText();

        foreach (Customer customer in customers)
        {
            StartCoroutine(RequestRoutine(customer));
        }
    }

    void Update()
    {
        UpdateSatisText();
    }

    public void DecreaseSatisfaction(float amount)
    {
        satisfaction -= amount;
        if (satisfaction <= 0)
        {
            satisfaction = 0;
            Debug.Log("Game Over!");
            gameOverText.text = "���ӿ���";
            gameOverText.gameObject.SetActive(true);
            // TODO: ���ӿ��� ó��
        }
        UpdateSatisText();
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
            yield return null;

            if (customer.requestData == null)
                continue;

            if (customer.requestData.requestType == RequestType.Ready)
            {
                customer.timer -= Time.deltaTime;

                if (customer.timer <= 0f)
                {
                    customer.isRequestActive = false;
                    if (currentActiveRequestCount < maxActiveRequests)
                    {
                        RequestData chosen = PickRandomRequest();
                        customer.AssignRequest(chosen);
                    }
                    else
                    {
                        customer.timer = 3f;
                        Debug.Log($"[{customer.name}] ��û ���� - ��� Ÿ�̸� �����");
                    }
                }
            }
        }
    }

    private RequestData PickRandomRequest()
    {
        int roll = Random.Range(0, 3);
        switch (roll)
        {
            case 0: return borrowBookRequest;
            case 1: return findBookRequest;
            case 2: return quietDownRequest;
            default: return borrowBookRequest;
        }
    }

    private void UpdateSatisText()
    {
        if (satisText != null)
            satisText.text = $"������: {Mathf.RoundToInt(satisfaction)}";
    }

    public void UpdateInventoryText()
    {
        if (StageGameManager.Instance.inventoryText == null) return;

        string result = "";

        if (PlayerInventory.Instance.IsHoldingBook())
        {
            result = $"å{PlayerInventory.Instance.GetHeldBookID()}";
        }
        else if (PlayerInventory.Instance.IsHoldingCustomerItem())
        {
            result = $"�մ�{PlayerInventory.Instance.heldCustomerID}å";
        }

        StageGameManager.Instance.inventoryText.text = result;
    }

}
