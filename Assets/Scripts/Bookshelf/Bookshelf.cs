using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookshelf : MonoBehaviour
{
    public int bookshelfID = 0;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("å ȹ��!");
                PlayerInventory.Instance.HoldBook(bookshelfID);
                Debug.Log($"å�� {bookshelfID}������ å�� ����");
            }
        }
    }
}
