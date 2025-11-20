using UnityEngine;
using System.Collections;

public class BulletBehavior : MonoBehaviour
{
    public float bulletLifetime = 5.0f;
    public float bulletSpeed = 10.0f;
    Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyAfterLifetime());
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
        {
            rb.linearVelocity = transform.right * bulletSpeed;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle player hit logic here
            Destroy(gameObject);
        }
    }
    IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(bulletLifetime);
        Destroy(gameObject);
    }
}
