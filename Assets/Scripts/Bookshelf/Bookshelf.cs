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
            PlayerInventory.Instance.tempBookID = bookshelfID; // 임시 저장
            Debug.Log($"플레이어 감지됨, 책장 {bookshelfID}번 입장");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (PlayerInventory.Instance.tempBookID == bookshelfID)
                PlayerInventory.Instance.tempBookID = -1; // 나가면 초기화
            Debug.Log($"플레이어 퇴장, 책장 {bookshelfID}번");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            int tempID = PlayerInventory.Instance.tempBookID;
            if (tempID != -1)
            {
                Debug.Log($"책 {tempID}번 획득!");
                PlayerInventory.Instance.HoldBook(tempID);
            }
        }
    }
}