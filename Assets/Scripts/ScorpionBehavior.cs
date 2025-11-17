using UnityEngine;
using System.Collections;

public class ScorpionBehavior : MonoBehaviour
{
    public float health = 10f;
    public float patrolRange = 5f;
    private float leftPoint;
    private float rightPoint;
    public float patrolSpeed = 2.0f;
    public float chargeSpeed = 4.0f;
    public float detectionRange = 5.0f;
    public float chargeCooldown = 3.0f;
    public LayerMask playerLayer;
    private Rigidbody2D rb;
    private Transform player;
    private bool isCharging = false;
    private bool canCharge = true;
    private Vector2 patrolDirection = Vector2.left;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D objectCollider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rightPoint = transform.position.x;
        leftPoint = rightPoint - patrolRange;
        animator = GetComponent<Animator>();
        objectCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCharging || !canCharge) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            StartCoroutine(Charge());
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        animator.ResetTrigger("Patrol");
        rb.linearVelocity = new Vector2(patrolDirection.x * patrolSpeed, rb.linearVelocity.y);
        // Turn around logic, subject to change
        //      likely to cause problems in current state
        if (transform.position.x <= leftPoint && patrolDirection.x < 0)
        {
            Flip();
        }
        else if (transform.position.x >= rightPoint && patrolDirection.x > 0)
        {
            Flip();
        }
    } 
    void Flip()
    {
        patrolDirection = -patrolDirection;
        Vector3 scale = transform.localScale;
        //scale.x *= -1;
        //transform.localScale = scale;
        spriteRenderer.flipX = !spriteRenderer.flipX;   
        Debug.Log("Scorpion flipped direction!");
    }
    IEnumerator Charge()
    {
        isCharging = true;
        canCharge = false;

        Vector2 chargeDirection = (player.position - transform.position).normalized;
        // flip if player is behind scorpion
        if (chargeDirection.x < 0 && patrolDirection.x > 0 || chargeDirection.x > 0 && patrolDirection.x < 0)
        {
            Flip();
        }

        rb.linearVelocity = new Vector2(chargeDirection.x * chargeSpeed, rb.linearVelocity.y);
        animator.SetTrigger("Charge");
        yield return new WaitForSeconds(1.5f); //charge duration
        rb.linearVelocity = Vector2.zero; //freeze after charge
        isCharging = false;
        yield return new WaitForSeconds(chargeCooldown); // cooldown before next charge
        canCharge = true;
        animator.ResetTrigger("Charge");
        animator.SetTrigger("Patrol");
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit by scorpion charge!");
        }
        else
        {
            if (isCharging)
            {
                // Stop charging on collision with other objects
                isCharging = false;
                rb.linearVelocity = Vector2.zero;
            }
            Flip();
        }
    }
    public void gotShot(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log("Scorpion took " + damageAmount + " damage!");
        StartCoroutine(HitEffect());
        if (health <= 0)
        {
            Die();
        }

    }
    IEnumerator HitEffect()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
    void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log("Scorpion died!");
        Destroy(objectCollider);
        Destroy(gameObject, 1f);
    }
}
