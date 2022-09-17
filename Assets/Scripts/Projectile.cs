using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private const float ProjectileSpeed = 10.0f;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.up * ProjectileSpeed, ForceMode.VelocityChange);
    }
    // Destroy projectile on first object hit
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
