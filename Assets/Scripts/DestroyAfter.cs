using System.Collections;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    private const float ProjectileLifetime = 1.0f;

    private IEnumerator m_coroutine;

    // Start a Coroutine to destroy the game object in X seconds
    void Start()
    {
        m_coroutine = DestroyAfterCoroutine(ProjectileLifetime);

        StartCoroutine(m_coroutine);
    }

    // Destroy the object afted 'lifetime' seconds
    IEnumerator DestroyAfterCoroutine(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
