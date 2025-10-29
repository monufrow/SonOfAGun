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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

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
    void onAttack()
    {
        rb.AddForce(new Vector2(0f, recoilForce), ForceMode2D.Impulse);
    }
}
