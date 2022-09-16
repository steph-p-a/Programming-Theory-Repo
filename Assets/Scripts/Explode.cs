using UnityEngine;

public class Explode : MonoBehaviour
{
    private void FixedUpdate()
    {
        // Just keep the explosion long enough to collide
        Destroy(gameObject);
    }

}
