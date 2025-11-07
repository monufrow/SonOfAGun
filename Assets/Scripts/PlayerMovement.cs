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
    public float shotDistance = 6f;
    public int bulletCount = 3;
    public float reloadTime = 2f;
    private bool isReloading = false;


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
        while (!isReloading)
        {
            isReloading = true;
            StartCoroutine(ReloadCoroutine());
            Vector3 direction = (mouseWorldPos - transform.position).normalized;
            rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);
            ShootBullets(direction);
        }
        // Apply recoil
        //Vector3 direction = (mouseWorldPos - transform.position).normalized;
        //rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);
        //ShootBullets(direction);
        //Debug.Log("Recoil applied");
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
            Debug.Log("Collided with Enemy");
            StartCoroutine(HitEffect());
        }
        if (collision.gameObject.CompareTag("Cactus"))
        {
            Debug.Log("Collided with Cactus");
            StartCoroutine(HitEffect());
        }
    }
    IEnumerator HitEffect()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    void ShootBullets(Vector3 direction)
    {
        Debug.Log("Shooting bullets");
        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 spreadDirection = Quaternion.AngleAxis(Random.Range(-10, 11), Vector3.forward) * direction;
            Vector3 bulletOffset = new Vector3(0, Random.Range(-3, 4) * .05f, 0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + bulletOffset, spreadDirection, shotDistance, LayerMask.GetMask("Enemy"));
            ShootBulletsDebug(spreadDirection, bulletOffset);
            if (hit.collider != null)
            {
                Debug.Log("Bullet hit: " + hit.collider.name);
                // Here you can add logic to deal damage to the hit object if it has a health component
            }
        }
    }
    void ShootBulletsDebug(Vector3 direction, Vector3 offset)
    {
        Debug.DrawRay(transform.position + offset, direction * shotDistance, Color.red, 2f, false);

        //Debug.DrawRay(transform.position + new Vector3(0, .1f, 0), Quaternion.AngleAxis(Random.Range(-2, 11), Vector3.forward) * direction * shotDistance, Color.green, 2f, false);
        //Debug.DrawRay(transform.position - new Vector3(0, .1f, 0), Quaternion.AngleAxis(Random.Range(-10, 3), Vector3.forward) * direction * shotDistance, Color.green, 2f, false);
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        yield return null;
        isReloading = false;
    }
}
