using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public int heldBookID = -1;
    public int tempBookID = -1; // 책장에 들어왔을 때 임시 저장
    public int heldCustomerID = -1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // 중복 방지
    }

    public void HoldBook(int bookID)
    {
        heldBookID = bookID;
        heldCustomerID = -1; // 반납 아이템은 버림
        Debug.Log($"책 {bookID}번을 소지했습니다.");
        StageGameManager.Instance.UpdateInventoryText();
    }

    public bool IsHoldingBook() => heldBookID != -1;
    public int GetHeldBookID() => heldBookID;

    public void RemoveBook()
    {
        Debug.Log($"책 {heldBookID}번을 내려놓습니다.");
        heldBookID = -1;
        StageGameManager.Instance.UpdateInventoryText();
    }

    public void HoldCustomerItem(int customerID)
    {
        heldCustomerID = customerID;
        heldBookID = -1; // 책은 버림
        Debug.Log($"손님 {customerID}의 반납 아이템을 소지했습니다.");
        StageGameManager.Instance.UpdateInventoryText();
    }

    public bool IsHoldingCustomerItem() => heldCustomerID != -1;

    public void RemoveCustomerItem()
    {
        Debug.Log($"손님 {heldCustomerID}의 아이템을 반납합니다.");
        heldCustomerID = -1;
        StageGameManager.Instance.UpdateInventoryText();
    }
}