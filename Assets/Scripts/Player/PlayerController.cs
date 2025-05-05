using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] float walkSpeed = 3.0f;
    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] float runMana = 10.0f;
    [SerializeField] bool isRunning = false;

    [Header("상호작용")]
    [SerializeField] float interactRadius = 1.0f;

    private Rigidbody2D rb;
    private Vector2 MoveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        rb.velocity = MoveInput * speed;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Customer"));
            if (hit)
            {
                Customer customer = hit.GetComponent<Customer>();
                customer?.Interact();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
