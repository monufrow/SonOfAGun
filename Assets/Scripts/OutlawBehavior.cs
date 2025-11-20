using UnityEngine;
using System.Collections;

public class OutlawBehavior : MonoBehaviour
{
    [SerializeField] private float shootInterval = 5.0f;
    [SerializeField] private GameObject bulletPrefab;
    public float health = 100.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootInterval);
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            health -= 34.0f;
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }else */if (collision.gameObject.CompareTag("Player"))
        {
            // Handle player collision logic here
            StartCoroutine(Die());
        }
    }
    IEnumerator Die()
    {
        // Play death animation or effects here
        Destroy(GetComponent<Rigidbody2D>());
        yield return new WaitForSeconds(0.5f); // Wait for animation to finish
        Destroy(gameObject);
    }
}
