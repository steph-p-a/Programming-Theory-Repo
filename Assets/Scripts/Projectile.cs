using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float projectileLifetime = 1.0f;

    private IEnumerator removeProjectile;

    // Start is called before the first frame update
    void Start()
    {
        removeProjectile = DestroyAfter(projectileLifetime);

        StartCoroutine(removeProjectile);
    }

    private IEnumerator DestroyAfter(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

}
