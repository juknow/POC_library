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
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            FailRequest();
        }

        if (requestData.requestType == RequestType.Reading)
        {
            CheckNoise(); // continuously noise check
        }


        if (requestTimerSlider != null && requestTimerSlider.gameObject.activeSelf)
        {
            requestTimerSlider.value = timer;
        }

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
                    AssignRandomBookID();
                    UpdateStatusText();
                }
                break;
            case RequestType.ReturnBook:
                Debug.Log("책 반납 요청입니다. 데스크에서 처리해야 합니다.");
                break;

            case RequestType.FindBook:
                Debug.Log("책을 찾아다 주세요.");
                RequestUIManager.Instance.ShowRequest(this); // 책을 선택하는 UI 띄우기
                break;

            case RequestType.QuietDown:
                Debug.Log("쉿! 해줬습니다.");
                CompleteRequest();
                break;

            case RequestType.Reading:
                Debug.Log("읽고 있는 중입니다. 조용히...");
                // 상호작용은 안 하고 소음 감지만
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
        isRequestActive = false;
        StageGameManager.Instance.OnRequestEnded();
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
                requestData = null;
                requiredBookID = -1;
                if (statusText != null)
                    statusText.text = ""; // 상태 텍스트 초기화
                Debug.Log($"{gameObject.name} → Ready 상태 진입");
                break;
        }
    }

    void UpdateStatusText()
    {
        statusText.text = $"책 {requiredBookID}번 주세요!";
    }

    void FailRequest()
    {
        isRequestActive = false;
        StageGameManager.Instance.OnRequestEnded();
        StageGameManager.Instance.DecreaseSatisfaction(requestData.satisfactionPenalty);

        if (requestTimerSlider != null)
            requestTimerSlider.gameObject.SetActive(false);

        requestData = null;
        requiredBookID = -1;

        if (statusText != null)
            statusText.text = "";
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

        if (requestTimerSlider != null)
        {
            requestTimerSlider.maxValue = requestData.timeLimit;
            requestTimerSlider.value = requestData.timeLimit;
            requestTimerSlider.gameObject.SetActive(true);
        }

        if (requestData.requestType == RequestType.BorrowBook)
        {
            AssignRandomBookID();
            UpdateStatusText();
        }

        if (statusText != null)
            statusText.text = requestData.description;

        Debug.Log($"[{gameObject.name}] 요청 시작: {requestData.requestType}");
    }

}
