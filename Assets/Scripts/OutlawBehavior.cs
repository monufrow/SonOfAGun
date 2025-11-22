using UnityEngine;
using System.Collections;

public class OutlawBehavior : EnemyBase
{
    [SerializeField] private float shootInterval = 5.0f;
    [SerializeField] private GameObject bulletPrefab;
    private SpriteRenderer sr;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ShootRoutine());
        sr = GetComponent<SpriteRenderer>();
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
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(50);
        }
    }
    public override void HitEffect()
    {
        StartCoroutine(HitEffectRoutine());
    }
    public override IEnumerator HitEffectRoutine()
    {
        sr.color = Color.yellow;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
        // really, this could be a void method that plays an animation or something
    }
    public override void Die()
    {
        StartCoroutine(DieRoutine());
    }
    public override IEnumerator DieRoutine()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
        // Play death animation or effects here
        Destroy(GetComponent<Rigidbody2D>());
        yield return new WaitForSeconds(0.5f); // Wait for animation to finish
        Destroy(gameObject);
    }
}
