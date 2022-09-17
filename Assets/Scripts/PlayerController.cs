using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float elevationSpeed;
    [SerializeField] float azimuthSpeed;
    [SerializeField] GameObject projectilePrefab;


    private Transform m_muzzle;
    private const float AzimuthMin = -60.0f;
    private const float AzimuthMax = 60.0f;
    private const float ElevationMin = 40.0f;
    private const float ElevationMax = 90.0f;

    private Vector2 m_RotationInput;
    private Vector2 m_Rotation;

    void Start()
    {
        m_Rotation = new Vector2(transform.localEulerAngles.x, transform.localEulerAngles.y);

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
        m_Rotation.y = Mathf.Clamp(m_Rotation.y, AzimuthMin, AzimuthMax);
        m_Rotation.x += rotationInput.y * scaledElevationSpeed;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x, ElevationMin, ElevationMax);

        transform.localEulerAngles = m_Rotation;
    }

    private void Fire()
    {
        if (projectilePrefab != null && m_muzzle != null)
        {
            Instantiate(projectilePrefab, m_muzzle.position, m_muzzle.rotation);
        }
    }
}
