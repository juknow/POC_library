using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageGameManager : MonoBehaviour
{
    public static StageGameManager Instance;
    [Header("손님 관련")]
    public List<Customer> customers; // 에디터에서 5명 넣기

    [Header("요청 제한")]
    public int maxActiveRequests = 3;

    [Header("만족도")]
    public float satisfaction = 100f;
    public TextMeshProUGUI satisText;
    public TextMeshProUGUI gameOverText;


    [Header("요청 종류")]
    public RequestData borrowBookRequest;
    public RequestData quietDownRequest;
    public RequestData readingRequest;
    public RequestData returnBookRequest;
    public RequestData readyRequest;
    public RequestData findBookRequest;

    [Header("소지품UI")]
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
            gameOverText.text = "게임오버";
            gameOverText.gameObject.SetActive(true);
            // TODO: 게임오버 처리
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
                        Debug.Log($"[{customer.name}] 요청 보류 - 대기 타이머 재시작");
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
            satisText.text = $"만족도: {Mathf.RoundToInt(satisfaction)}";
    }

    public void UpdateInventoryText()
    {
        if (StageGameManager.Instance.inventoryText == null) return;

        string result = "";

        if (PlayerInventory.Instance.IsHoldingBook())
        {
            result = $"책{PlayerInventory.Instance.GetHeldBookID()}";
        }
        else if (PlayerInventory.Instance.IsHoldingCustomerItem())
        {
            result = $"손님{PlayerInventory.Instance.heldCustomerID}책";
        }

        StageGameManager.Instance.inventoryText.text = result;
    }

}
