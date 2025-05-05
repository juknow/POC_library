using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public RequestData requestData;
    public bool isRequestActive = true;

    [SerializeField] private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = requestData.timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            FailRequest();
        }
        
    }

    public void Interact()
    {
        if (!isRequestActive) return;

    }

    public void CompleteRequest()
    {
        isRequestActive = false;
        Destroy(gameObject);
    }

    void FailRequest()
    {
        isRequestActive = false;
        // StageGameManager.Instance.Decrease(requestData.sataisfactionPenalty); // 도서관 만족도 감소
        Destroy(gameObject);
    }
}
