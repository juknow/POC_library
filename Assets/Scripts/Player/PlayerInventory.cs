using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public int heldBookID = -1;
    public int tempBookID = -1; // å�忡 ������ �� �ӽ� ����
    public int heldCustomerID = -1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // �ߺ� ����
    }

    public void HoldBook(int bookID)
    {
        heldBookID = bookID;
        heldCustomerID = -1; // �ݳ� �������� ����
        Debug.Log($"å {bookID}���� �����߽��ϴ�.");
        StageGameManager.Instance.UpdateInventoryText();
    }

    public bool IsHoldingBook() => heldBookID != -1;
    public int GetHeldBookID() => heldBookID;

    public void RemoveBook()
    {
        Debug.Log($"å {heldBookID}���� ���������ϴ�.");
        heldBookID = -1;
        StageGameManager.Instance.UpdateInventoryText();
    }

    public void HoldCustomerItem(int customerID)
    {
        heldCustomerID = customerID;
        heldBookID = -1; // å�� ����
        Debug.Log($"�մ� {customerID}�� �ݳ� �������� �����߽��ϴ�.");
        StageGameManager.Instance.UpdateInventoryText();
    }

    public bool IsHoldingCustomerItem() => heldCustomerID != -1;

    public void RemoveCustomerItem()
    {
        Debug.Log($"�մ� {heldCustomerID}�� �������� �ݳ��մϴ�.");
        heldCustomerID = -1;
        StageGameManager.Instance.UpdateInventoryText();
    }
}