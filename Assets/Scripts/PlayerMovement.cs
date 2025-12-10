using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using NUnit.Framework.Internal;

public class PlayerMovement : MonoBehaviour
{
    public float playerHeight;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float recoilForce = 20f;
    [SerializeField]
    private LayerMask groundLayer;
    private Rigidbody2D rb;
    Vector3 mouseWorldPos;
    Vector2 mouseScreenPos;
    public SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    public GameObject crosshair;
    [SerializeField] private Material playerMaterial;
    public float shotDistance = 6f;
    public int bulletCount = 3;
    public float reloadTime = 2f;
    private float horizontalInput;
    private bool isReloading = false;
    public GameObject gun;
    private Vector2 moveInput;
    public Image reloadCircle;
    public int lives = 3;
    private Vector3 startPosition;
    [SerializeField] private GameObject BulletPrefab;
    
    private int layerToIgnore;
    private Animator animator;
    private bool isCrouched = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMaterial = GetComponent<SpriteRenderer>().material;
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        layerToIgnore = 1 << LayerMask.NameToLayer("Player");

    }

    // Update is called once per frame
    void Update()
    {
        crosshair.transform.position = mouseScreenPos;
        MouseMove();
        Vector2 targetVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        //if (rb.linearVelocity == Vector2.zero || rb.linearVelocity == targetVelocity)
        // {
        //     rb.linearVelocity = targetVelocity;
        // }
        // else
        // {
        //     rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 0.0075f);
        // }
        if (Mathf.Abs(rb.linearVelocity.x) > Mathf.Abs(targetVelocity.x))
        {
            // lerp wil interpolate between two vectors, when the float is closer to 0 it will
            // prefer to stay closer to the first vector. This will bring the player back to walking speed over time.
            // its kinda quick but this was the best option I found, either it instantly went back to walking speed, or the player slid too long.
            // between 0.01 and 0.005 were good values I found.
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 0.0075f);
        }
        else
        {
            rb.linearVelocity = targetVelocity;
        }
        animator.SetFloat("XVelo", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("YVelo", rb.linearVelocity.y);
        animator.SetBool("isGrounded",IsGrounded());
        animator.SetBool("isCrouched",isCrouched);
    }

    void FixedUpdate()
    {
        // ... (your movement physics here) ...

        // Flip the sprite based on input direction
        if (rb.linearVelocityX < 0)
        {
            spriteRenderer.flipX = true;
        }
        if (rb.linearVelocityX > 0)
        {
            spriteRenderer.flipX = false;
        }

        Vector3 direction = (mouseWorldPos - transform.position).normalized;
        Vector3 spreadDirection = Quaternion.AngleAxis(Random.Range(-10, 11), Vector3.forward) * direction;
        gun.transform.rotation = Quaternion.LookRotation(Vector3.forward, spreadDirection);
    }
    void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (IsGrounded())
                isCrouched = true;
        }
        else
        {
            if (isCrouched)  // don't check grounded again
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            isCrouched = false;
        }
    }


    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, playerHeight, groundLayer);
        return hit.collider != null;
    }
    void OnAttack()
    {
        if (!isReloading)
        {
            isReloading = true;
            StartCoroutine(ReloadCoroutine());
            Vector3 direction = (mouseWorldPos - transform.position).normalized;
            ShootBullets(direction);
            direction.x *= 2.5f;
            rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);
        }
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

    void ShootBullets(Vector3 bulletDrection)
    {
        //Debug.Log("Shooting bullets");
        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 spreadDirection = Quaternion.AngleAxis(Random.Range(-10, 11), Vector3.forward) * bulletDrection;
            int layerMask = ~layerToIgnore;
            Vector3 bulletOffset = new Vector3(0, Random.Range(-3, 4) * .05f, 0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + bulletOffset, spreadDirection, shotDistance, layerMask); //LayerMask.GetMask("Enemy")
            ShootBulletsDebug(spreadDirection, bulletOffset);
            Instantiate(BulletPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, spreadDirection));
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    GameObject hitEnemy = hit.collider.gameObject;
                    Destroy(hitEnemy);
                }
                //Debug.Log("Bullet hit: " + hit.collider.name);
                EnemyBase enemy = hit.collider.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(34);
                    enemy.HitEffect();
                    if (enemy.health <= 0)
                    {
                        enemy.Die();
                    }
                }
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
        isReloading = true;
        reloadCircle.gameObject.SetActive(true);
        reloadCircle.fillAmount = 0f;
        float elapsed = 0f;
        while (elapsed < reloadTime)
        {
            elapsed += Time.deltaTime;
            reloadCircle.fillAmount = elapsed / reloadTime;
            yield return null;
        }
        reloadCircle.fillAmount = 0f;
        yield return null;
        isReloading = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("Collided with Enemy");
            StartCoroutine(HitEffect());
        }
        else if (collision.gameObject.CompareTag("Cactus"))
        {
            Debug.Log("Collided with Cactus");
            StartCoroutine(HitEffect());
        }
        else if (collision.gameObject.CompareTag("InstantDeath"))
        {
            Debug.Log("Collided with Instant Death object");
            Respawn();
        }else if (collision.gameObject.CompareTag("Goal"))
        {
            Debug.Log("Player reached the goal!");
            //GameManager.Instance.LevelComplete();
        }
    }
    IEnumerator HitEffect()
    {
        LoseLife();
        rb.AddForce(new Vector2(-rb.linearVelocity.x, jumpForce / 2), ForceMode2D.Impulse);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
    void LoseLife()
    {
        lives--;
        GameManager.Instance.LoseLife();
        //Debug.Log("Player lost a life!");
        if (lives <= 0)
        {
            //Debug.Log("Player has died!");
            Respawn();
        }
    }
    void Respawn()
    {
        transform.position = startPosition;
        lives = 3;
        GameManager.Instance.RestoreLives();
    }
}
