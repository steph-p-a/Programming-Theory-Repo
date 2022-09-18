﻿using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float elevationSpeed;
    [SerializeField] float azimuthSpeed;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject crosshair;

    private Transform m_muzzle;

    private const float AzimuthMin = -60.0f;
    private const float AzimuthMax = 60.0f;
    private const float ElevationMin = 40.0f;
    private const float ElevationMax = 95.0f;
    private const float ProjectileSpeed = 10.0f;
    private float m_Azimuth; // Rotation in Euleur degrees around the Y axis, where axis Z is 0
    private float m_Elevation; // Angle in Euleur degrees from the Y axis toward the XZ plane, where the Y axis is 0
                               // Normally elevation is degrees above horizon (XZ plane), but in Unity 0 deg is 'up'.

    private Vector2 m_RotationInput; // .x controls the azimuth, .y controls the elevation

    void Start()
    {
        m_Azimuth = transform.eulerAngles.y;
        m_Elevation = transform.eulerAngles.x;

        m_muzzle = GameObject.Find("Muzzle").GetComponent<Transform>();
        if (m_muzzle == null)
        {
            Debug.LogError("Missing Muzzle GameObject");
        }
        if (projectilePrefab)
        {
            if (projectilePrefab.GetComponent<Rigidbody>() == null)
            {
                Debug.LogError("ProjectilePrefab: " + projectilePrefab.name + " does not have a RigidBody component");
            }
        }
        else
        {
            Debug.LogError("PlayerController does not have a projectile prefab");
        }
    }

    void Update()
    {
        Rotate(m_RotationInput);
        DrawCrosshair3D();
    }

    public void OnRotationInput(InputAction.CallbackContext context)
    {
        m_RotationInput = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Fire();
        }
    }

    private void Rotate(Vector2 rotationInput)
    {
        var scaledElevationSpeed = elevationSpeed * Time.deltaTime;
        var scaledAzimuthSpeed = azimuthSpeed * Time.deltaTime;

        m_Azimuth += rotationInput.x * scaledAzimuthSpeed;
        m_Azimuth = Mathf.Clamp(m_Azimuth, AzimuthMin, AzimuthMax);
        m_Elevation += rotationInput.y * scaledElevationSpeed;
        m_Elevation = Mathf.Clamp(m_Elevation, ElevationMin, ElevationMax);

        transform.rotation = Quaternion.Euler(m_Elevation, m_Azimuth, 0.0f);
    }

    private void Fire()
    {
        if (projectilePrefab != null && m_muzzle != null)
        {
            var projectile = Instantiate(projectilePrefab, m_muzzle.position, m_muzzle.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(transform.up * ProjectileSpeed, ForceMode.VelocityChange);
        }
    }


    // ABSTRACTION : Most users of this function will not really care about how it's implemented, they only care that it works
    /*
     * DrawCrosshair3D paints a crosshair at a predicted projectile hit location, given the projectile speed vector.
     * Assumptions:
     *   - the drag on the projectile is negligible, thus ignored
     *   - the gravity is toward negative Y axis
     *   - "Muzzle" being a cylinder, it's the direction of the long axis is 'up'
     *   - The hit location normal is always in the -Z direction, so we don't need to rotate the crosshair image.
     * 
     * These assumptions allow us to use a Raycast on the XZ plane.
     * DrawCrosshair3D uses eliptical projectile equation: y(t) = y0 + (initialSpeed.y * t) + (1/2 * g * t * t);
     * Where y0 is the y component of the muzzle position
     *       t is the time
     *       g is negative (-9.8 m/ss)       
     */
    private void DrawCrosshair3D()
    {
        Vector3 projectileSpeedVector = m_muzzle.transform.TransformDirection(Vector3.up) * ProjectileSpeed;

        RaycastHit hit;
        if (Physics.Raycast(m_muzzle.transform.position, projectileSpeedVector, out hit))
        {
            float projectileSpeedXZ = Vector3.ProjectOnPlane(projectileSpeedVector, Vector3.up).magnitude;

            if (projectileSpeedXZ != 0.0f)
            {
                var deltaT = hit.distance / projectileSpeedXZ;

                var y = m_muzzle.transform.position.y + (projectileSpeedVector.y * deltaT) + (0.5 * Physics.gravity.y * deltaT * deltaT);
                crosshair.transform.position = new Vector3(hit.point.x, (float)y, hit.point.z);
            }
        }
    }
}
