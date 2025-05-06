using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public int heldBookID = -1; // -1 is no book

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void HoldBook(int bookID)
    {
        heldBookID = bookID;
        Debug.Log($"책 {bookID}번을 소지했습니다.");
    }


    public bool IsHoldingBook()
    {
        return heldBookID != -1;
    }

    public int GetHeldBookID()
    {
        return heldBookID;
    }

    public void RemoveBook()
    {
        Debug.Log($"책 {heldBookID}번을 내려놓습니다.");
        heldBookID = -1;
    }
}