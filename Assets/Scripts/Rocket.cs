using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Rocket : MonoBehaviour
{
    private const float RocketSpeed = 3.0f;

    private void Start()
    {
        var rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.up * RocketSpeed, ForceMode.VelocityChange);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform != transform.parent)
        {
            Destroy(gameObject);
        }
    }

}
