﻿using UnityEngine;

public class Desk : MonoBehaviour
{

    [Header("반납 대상 손님 목록")]
    public Customer[] customers; // 인스펙터에서 드래그할 배열

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            foreach (Customer customer in customers)
            {
                if (customer != null && customer.isRequestActive &&
                    customer.requestData.requestType == RequestType.ReturnBook)
                {
                    int heldID = PlayerInventory.Instance.heldCustomerID;

                    if (heldID == customer.customerID)
                    {
                        Debug.Log($"반납 완료: 손님 {heldID}");
                        customer.CompleteRequest();
                        PlayerInventory.Instance.RemoveCustomerItem();
                        break;
                    }
                }
            }
        }
    }
}