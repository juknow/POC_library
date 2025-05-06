using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] float walkSpeed = 3.0f;
    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] float maxRunMana = 10.0f;
    [SerializeField] float runManaDrainRate = 2.0f;
    [SerializeField] float runManaRegenRate = 1.5f;
    private float currentRunMana;
    [SerializeField] bool isRunning = false;

    [Header("상호작용")]
    [SerializeField] float interactRadius = 0.5f;

    [Header("UI")]
    [SerializeField] Slider runManaSlider;

    private Rigidbody2D rb;
    private Vector2 MoveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        void Start()
        {
            currentRunMana = maxRunMana;
            if (runManaSlider != null)
            {
                runManaSlider.maxValue = maxRunMana;
                runManaSlider.value = currentRunMana;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        ////// Run Logic

        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);
        isRunning = shiftHeld && currentRunMana > 0;

        float speed = isRunning ? runSpeed : walkSpeed;
        rb.velocity = MoveInput * speed;

        HandleRunMana(shiftHeld);

        //////

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

    void HandleRunMana(bool tryingToRun)
    {
        if (tryingToRun && MoveInput != Vector2.zero)
        {
            currentRunMana -= runManaDrainRate * Time.deltaTime;
            currentRunMana = Mathf.Max(0, currentRunMana);
        }
        else
        {
            currentRunMana += runManaRegenRate * Time.deltaTime;
            currentRunMana = Mathf.Min(maxRunMana, currentRunMana);
        }

        if (runManaSlider != null)
            runManaSlider.value = currentRunMana;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
