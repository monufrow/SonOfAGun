using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float recoilForce = 20f;
    [SerializeField]
    private LayerMask groundLayer;
    private Rigidbody2D rb;
    Vector3 mouseWorldPos;
    Vector2 mouseScreenPos;
    public GameObject crosshair;
    [SerializeField] private Material playerMaterial;
    SpriteRenderer spriteRenderer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMaterial = GetComponent<SpriteRenderer>().material;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        crosshair.transform.position = mouseScreenPos;
        MouseMove();
    }
    void OnMove(InputValue value)
    {
        Vector2 movement = new Vector2(value.Get<Vector2>().x * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = movement;
    }
    void OnJump()
    {
        if (IsGrounded())
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        return hit.collider != null;
    }
    void OnAttack()
    {
        // Apply recoil
        Vector3 direction = (mouseWorldPos - transform.position).normalized;
        rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);
        Debug.Log("Recoil applied");
    }
    void MouseMove()
    {
        if (Mouse.current != null)
        {
            mouseScreenPos = Mouse.current.position.ReadValue();
            mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0f));
            mouseWorldPos.z = 0f;

            crosshair.transform.position = mouseWorldPos;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Tumbleweed");
            StartCoroutine(HitEffect());
        }
    }
    IEnumerator HitEffect()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}
