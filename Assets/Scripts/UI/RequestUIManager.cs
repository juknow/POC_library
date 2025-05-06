using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestUIManager : MonoBehaviour
{
    public static RequestUIManager Instance;

    public GameObject requestPanel;
    public Text requestText;
    private Customer currentCustomer;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowRequest(Customer customer)
    {
        currentCustomer = customer;
        requestText.text = customer.requestData.description;
        requestPanel.SetActive(true);
    }
}
