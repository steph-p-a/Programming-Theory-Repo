using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float m_projectileLifetime = 1.0f;

    private IEnumerator m_removeProjectile;

    // Start is called before the first frame update
    void Start()
    {
        m_removeProjectile = DestroyAfter(m_projectileLifetime);

        StartCoroutine(m_removeProjectile);
    }

    IEnumerator DestroyAfter(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
