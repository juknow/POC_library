using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public RequestData requestData;
    public bool isRequestActive = false;
    public int requiredBookID = -1;

    public int customerID;
    public float timer;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] Slider requestTimerSlider;
    // Start is called before the first frame update
    void Start()
    {
        timer = requestData.timeLimit;

        requestTimerSlider.maxValue = requestData.timeLimit;
        requestTimerSlider.value = requestData.timeLimit;
        requestTimerSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (requestData == null) return;

        timer -= Time.deltaTime;

        if (timer <= 0f &&
            requestData.requestType != RequestType.Ready &&
            requestData.requestType != RequestType.Reading)
        {
            FailRequest();
            return;
        }

        if (requestData.requestType == RequestType.Reading)
        {
            CheckNoise();
            if (timer <= 0f)
            {
                Debug.Log($"[{customerID}] 책 다 읽었어요! 반납하러 갑니다.");
                CompleteRequest(); // → Reading이 끝나면 ReturnBook 상태로 전환
            }
        }

        if (requestTimerSlider != null && requestTimerSlider.gameObject.activeSelf)
        {
            requestTimerSlider.value = timer;
        }

        UpdateStatusText();
    }

    public void Interact()
    {
        if (!isRequestActive) return;

        //RequestUIManager.Instance.ShowRequest(this);

        switch (requestData.requestType)
        {
            case RequestType.BorrowBook:
                if (requiredBookID == -1) // 요청 처음 시작일 때만
                {
                }
                break;
            case RequestType.ReturnBook:
                if (!PlayerInventory.Instance.IsHoldingCustomerItem())
                {
                    PlayerInventory.Instance.HoldCustomerItem(customerID);
                    Debug.Log($"손님 {customerID}의 반납 아이템을 소지했습니다.");
                }
                else
                {
                    Debug.Log("이미 반납 아이템을 들고 있습니다.");
                }
                break;

            case RequestType.FindBook:
                if (PlayerInventory.Instance.IsHoldingBook())
                {
                    int held = PlayerInventory.Instance.GetHeldBookID();
                    if (held == requiredBookID)
                    {
                        Debug.Log($"책 {held}번 정답입니다! 요청 완료 (FindBook)");
                        PlayerInventory.Instance.RemoveBook();
                        CompleteRequest(); // → Ready로 전환
                    }
                    else
                    {
                        Debug.Log($"책 {held}번은 틀렸습니다. 필요한 책은 {requiredBookID}번입니다.");
                    }
                }
                else
                {
                    Debug.Log("책이 없습니다!");
                }
                break;

            case RequestType.QuietDown:
                Debug.Log("쉿! 해줬습니다.");
                CompleteRequest();
                break;

            case RequestType.Reading:
                Debug.Log("읽고 있는 중입니다. 조용히...");
                // 상호작용은 안 하고 소음 감지만
                break;
            case RequestType.Ready:
                timer = 10.0f;
                break;
        }


        if (requestData.requestType == RequestType.BorrowBook)
        {
            if (PlayerInventory.Instance.IsHoldingBook())
            {
                int held = PlayerInventory.Instance.GetHeldBookID();

                if (held == requiredBookID)
                {
                    Debug.Log($"책 {held}번 정답입니다! 요청 완료");
                    PlayerInventory.Instance.RemoveBook();
                    CompleteRequest(); // → Reading으로 전환됨
                }
                else
                {
                    Debug.Log($"책 {held}번은 틀렸습니다! 필요한 책은 {requiredBookID}번입니다.");
                    //  요청은 유지됨
                }
            }
            else
            {
                Debug.Log("책이 없어요!");
            }
        }
    }
    void AssignRandomBookID()
    {
        requiredBookID = Random.Range(1, 6); // 1~5
        Debug.Log($"손님이 요청한 책 번호: {requiredBookID}");
    }

    public void CompleteRequest()
    {
        if (isRequestActive)
            StageGameManager.Instance.OnRequestEnded();
        isRequestActive = false;
        requestTimerSlider.gameObject.SetActive(false);

        switch (requestData.requestType)
        {
            case RequestType.BorrowBook:
                AssignRequest(StageGameManager.Instance.readingRequest); // 책 읽기 상태로 전환
                break;

            case RequestType.Reading:
                AssignRequest(StageGameManager.Instance.returnBookRequest); // 책 반납 상태로 전환
                break;

            case RequestType.ReturnBook:
                AssignRequest(StageGameManager.Instance.readyRequest); // 명시적으로 Ready 상태 지정
                break;
            case RequestType.QuietDown:
                AssignRequest(StageGameManager.Instance.readyRequest); // 명시적으로 Ready 상태 지정
                break;
            case RequestType.FindBook:
                AssignRequest(StageGameManager.Instance.readyRequest); // 명시적으로 Ready 상태 지정
                break;
        }
    }


    void FailRequest()
    {
        if (isRequestActive)
            StageGameManager.Instance.OnRequestEnded();
        isRequestActive = false;
        StageGameManager.Instance.DecreaseSatisfaction(requestData.satisfactionPenalty);

        if (requestTimerSlider != null)
            requestTimerSlider.gameObject.SetActive(false);

        AssignRequest(StageGameManager.Instance.readyRequest);
        requiredBookID = -1;


    }

    void CheckNoise()
    {
        // TODO: noisy customer check and satisfaction lower.
    }

    public void AssignRequest(RequestData newRequest)
    {
        if (isRequestActive) return;

        requestData = newRequest;
        isRequestActive = true;
        timer = requestData.timeLimit;

        if (requestData.requestType != RequestType.Ready)
        {
            StageGameManager.Instance.OnRequestStarted();
        }

        if (requestTimerSlider != null)
        {
            requestTimerSlider.maxValue = requestData.timeLimit;
            requestTimerSlider.value = requestData.timeLimit;
            requestTimerSlider.gameObject.SetActive(true);
        }

        if (requestData.requestType == RequestType.BorrowBook ||
            requestData.requestType == RequestType.FindBook)
        {
            AssignRandomBookID();

        }

        switch (requestData.requestType)
        {
            case RequestType.Ready:
                timer = 10;
                statusText.text = "심심하다...";
                requestTimerSlider?.gameObject.SetActive(false);
                return;

            case RequestType.BorrowBook:
                AssignRandomBookID();
                statusText.text = $"책 {requiredBookID} 꺼내줘";
                break;

            case RequestType.Reading:
                statusText.text = $"책 {requiredBookID} 읽는 중";
                break;

            case RequestType.ReturnBook:
                statusText.text = "이거 반납할래";
                break;

            case RequestType.QuietDown:
                statusText.text = "시끌시끌";
                break;

            case RequestType.FindBook:
                AssignRandomBookID();
                statusText.text = $"책 {requiredBookID} 찾아줘";
                break;
        }

        Debug.Log($"[{gameObject.name}] 요청 시작: {requestData.requestType}");

    }

    void UpdateStatusText()
    {
        switch (requestData.requestType)
        {
            case RequestType.Ready:
                statusText.text = "심심하다...";
                break;

            case RequestType.BorrowBook:
                statusText.text = $"책 {requiredBookID} 꺼내줘";
                break;

            case RequestType.Reading:
                statusText.text = $"책 {requiredBookID} 읽는 중";
                break;

            case RequestType.ReturnBook:
                statusText.text = "이거 반납할래";
                break;

            case RequestType.QuietDown:
                statusText.text = "시끌시끌";
                break;

            case RequestType.FindBook:
                statusText.text = $"책 {requiredBookID} 찾아줘";
                break;
        }
    }

}
