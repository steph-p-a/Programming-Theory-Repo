using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float elevationSpeed;
    [SerializeField] float azimuthSpeed;
    [SerializeField] GameObject projectilePrefab;


    private Transform m_muzzle;
    private const float m_projectileSpeed = 10.0f;
    private const float m_azimuthMin = -60.0f;
    private const float m_azimuthMax = 60.0f;
    private const float m_elevationMin = 40.0f;
    private const float m_elevationMax = 90.0f;

    private Vector2 m_RotationInput;
    private Vector2 m_Rotation;


    // Start is called before the first frame update
    void Start()
    {
        m_Rotation = new Vector2(transform.localEulerAngles.x, transform.localEulerAngles.y);

        m_muzzle = GameObject.Find("Muzzle").GetComponent<Transform>();
        if (m_muzzle == null)
        {
            Debug.LogError("Missing Muzzle GameObject");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rotate(m_RotationInput);
    }

    public void OnRotate(InputAction.CallbackContext context)
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

        m_Rotation.y += rotationInput.x * scaledAzimuthSpeed;
        m_Rotation.y = Mathf.Clamp(m_Rotation.y, m_azimuthMin, m_azimuthMax);
        m_Rotation.x += rotationInput.y * scaledElevationSpeed;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x, m_elevationMin, m_elevationMax);

        transform.localEulerAngles = m_Rotation;
    }

    private void Fire()
    {
        if (projectilePrefab != null && m_muzzle != null)
        {
            var newProjectile = Instantiate(projectilePrefab, m_muzzle.position, m_muzzle.rotation);
            var newProjectileRb = newProjectile.GetComponent<Rigidbody>();
            if (newProjectileRb != null)
            {
                newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.up * m_projectileSpeed, ForceMode.VelocityChange);
            }
            else
            {
                Debug.LogError("ProjectilePrefab: " + projectilePrefab.name + " does not have a RigidBody component");
            }
        }
    }

}
