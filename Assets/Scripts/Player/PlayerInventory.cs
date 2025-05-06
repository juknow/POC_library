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
        Debug.Log($"å {bookID}���� �����߽��ϴ�.");
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
        Debug.Log($"å {heldBookID}���� ���������ϴ�.");
        heldBookID = -1;
    }
}