using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour
{
    public int health;

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0){
            Die();
        }
        else
        {
            HitEffect();
        }
    }
    public virtual void HitEffect()
    {
        // Default hit effect (can be overridden)
    }
    public virtual IEnumerator HitEffectRoutine()
    {
        yield return null;
    }
    public virtual void Die()
    {
        Destroy(gameObject);
    }
    public virtual IEnumerator DieRoutine()
    {
        Destroy(gameObject);
        yield return null;
    }
}
