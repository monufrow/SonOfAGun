using UnityEngine;
using System.Collections;


public class TumbleweedMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float baseSpeed;
    public float speedVariance;
    public float rotationMultiplier;
    public bool bounceEnabled = true;
    public float bounceForce;
    private float currentSpeed;
    public Vector2 moveDirection = new Vector2(-1f, 0f);
    public float lifetime = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        // randomize speed and direction
        currentSpeed = baseSpeed + Random.Range(-speedVariance, speedVariance);


        rb.linearVelocity = moveDirection * currentSpeed;
        StartCoroutine(DespawnAfterTime());
    }


    // Update is called once per frame
    void Update()
    {
        float rotation = rb.linearVelocity.x * rotationMultiplier * Time.deltaTime;
        transform.Rotate(0f, 0f, -rotation);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (bounceEnabled)
        {
            // Reflect direction on collision
            Vector2 reflectDir = Vector2.Reflect(rb.linearVelocity.normalized, collision.contacts[0].normal);
            rb.linearVelocity = reflectDir * currentSpeed;
            if (collision.gameObject.CompareTag("Ground"))
            {
                rb.AddForce(Vector2.up * currentSpeed * bounceForce, ForceMode2D.Impulse);
                bounceForce *= 0.9f;
                Debug.Log("Tumbleweed bounced off ground");
            }
            else
            {
                Debug.Log("Tumbleweed bounced off object");
            }
        }


    }
   
    IEnumerator DespawnAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
   
}
