using UnityEngine;

public class GunSrAnimationManager : MonoBehaviour
{
    GameObject parentGameObject;
    private Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentGameObject = transform.parent.gameObject;
        rb = parentGameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.linearVelocityX < 0)
        {
            spriteRenderer.flipX = true;
            transform.localPosition = new Vector3(-0.575f, 0.125f, -0.001f); //flipped
        }
        if (rb.linearVelocityX > 0)
        {
            spriteRenderer.flipX = false;
            transform.localPosition = new Vector3(0.575f, 0.125f, -0.001f);
        }
    }
}
