using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Rocket : MonoBehaviour
{
    private const float RocketSpeed = 10.0f;

    private void Start()
    {
        var rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.up * RocketSpeed, ForceMode.VelocityChange);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
