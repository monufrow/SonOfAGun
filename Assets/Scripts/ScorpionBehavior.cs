using UnityEngine;
using System.Collections;

public class ScorpionBehavior : MonoBehaviour
{
    public float patrolSpeed = 2.0f;
    public float chargeSpeed = 4.0f;
    public float detectionRange = 5.0f;
    public float chargeCooldown = 3.0f;
    public LayerMask playerLayer;
    private Rigidbody2D rb;
    private Transform player;
    private bool isCharging = false;
    private bool canCharge = true;
    private Vector2 patrolDirection = Vector2.right;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        // patrol direction is negative so it starts by moving left
        rb.linearVelocity = new Vector2(-patrolDirection.x * patrolSpeed, rb.linearVelocity.y);
        // Turn around logic, subject to change
        //      likely to cause problems in current state
        RaycastHit2D hit = Physics2D.Raycast(transform.position, patrolDirection, 0.5f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            Flip();
        }
    } 
    void Flip()
    {
        patrolDirection = -patrolDirection;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    IEnumerator Charge()
    {
        isCharging = true;
        canCharge = false;

        Vector2 chargeDirection = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(chargeDirection.x * chargeSpeed, rb.linearVelocity.y);

        yield return new WaitForSeconds(1.5f); //charge duration
        rb.linearVelocity = Vector2.zero; //freeze after charge
        isCharging = false;
        yield return new WaitForSeconds(chargeCooldown); // cooldown before next charge
        canCharge = true;
    }
}
