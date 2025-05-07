using UnityEngine;

public class Bookshelf : MonoBehaviour
{
    public int bookshelfID;

    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            PlayerInventory.Instance.tempBookID = bookshelfID; // �ӽ� ����
            Debug.Log($"�÷��̾� ������, å�� {bookshelfID}�� ����");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (PlayerInventory.Instance.tempBookID == bookshelfID)
                PlayerInventory.Instance.tempBookID = -1; // ������ �ʱ�ȭ
            Debug.Log($"�÷��̾� ����, å�� {bookshelfID}��");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            int tempID = PlayerInventory.Instance.tempBookID;
            if (tempID != -1)
            {
                Debug.Log($"å {tempID}�� ȹ��!");
                PlayerInventory.Instance.HoldBook(tempID);
            }
        }
    }
}