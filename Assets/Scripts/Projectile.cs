﻿using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Destroy projectile on first object hit
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
