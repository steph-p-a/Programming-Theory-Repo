using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float ProjectileLifetime = 1.0f;

    private IEnumerator m_removeProjectile;

    // Start is called before the first frame update
    void Start()
    {
        m_removeProjectile = DestroyAfter(ProjectileLifetime);

        StartCoroutine(m_removeProjectile);
    }

    IEnumerator DestroyAfter(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
