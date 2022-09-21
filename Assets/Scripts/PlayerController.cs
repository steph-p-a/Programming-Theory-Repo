using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject crosshair;
    [SerializeField] GameObject gunSmoke;

    private Transform m_muzzle;

    private const float AzimuthMin = -60.0f;
    private const float AzimuthMax = 60.0f;
    private const float AzimuthMaxSpeed = 120.0f;
    private const float AzimuthAcceleration = 120.0f;
    private const float ElevationMin = 40.0f;
    private const float ElevationMax = 95.0f;
    private const float ElevationMaxSpeed = 60.0f;
    private const float ElevationAcceleration = 60.0f;

    private const float ProjectileSpeed = 10.0f;

    private float m_AzimuthSpeed;
    private float m_ElevationSpeed;
    private float m_Azimuth; // Rotation in Euleur degrees around the Y axis, where axis Z is 0
    private float m_Elevation; // Angle in Euleur degrees from the Y axis toward the XZ plane, where the Y axis is 0
                               // Normally elevation is degrees above horizon (XZ plane), but in Unity 0 deg is 'up'.

    private Vector2 m_RotationInput; // .x controls the azimuth, .y controls the elevation


    private Plane m_Plane; // Used as a reference for crosshair prediction

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
        if (gunSmoke == null)
        {
            Debug.LogError("PlayerController does not have gunSmoke");
        }

        m_Plane = new Plane(Vector3.back, GameManager.Instance.TargetZPosition);
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
        if (GameManager.Instance.IsGameRunning && context.phase == InputActionPhase.Performed)
        {
            Fire();
        }
    }

    private void Rotate(Vector2 rotationInput)
    {
        float targetElevationSpeed = rotationInput.y * ElevationMaxSpeed;
        m_ElevationSpeed += (targetElevationSpeed - m_ElevationSpeed) * ElevationAcceleration * Time.deltaTime;
        m_ElevationSpeed = Mathf.Clamp(m_ElevationSpeed, -ElevationMaxSpeed, ElevationMaxSpeed);
        m_Elevation += m_ElevationSpeed * Time.deltaTime;
        m_Elevation = Mathf.Clamp(m_Elevation, ElevationMin, ElevationMax);

        float targetAzimuthSpeed = rotationInput.x * AzimuthMaxSpeed;
        m_AzimuthSpeed += (targetAzimuthSpeed - m_AzimuthSpeed) * AzimuthAcceleration * Time.deltaTime;
        m_AzimuthSpeed = Mathf.Clamp(m_AzimuthSpeed, -AzimuthMaxSpeed, AzimuthMaxSpeed);
        m_Azimuth += m_AzimuthSpeed * Time.deltaTime;
        m_Azimuth = Mathf.Clamp(m_Azimuth, AzimuthMin, AzimuthMax);

        transform.rotation = Quaternion.Euler(m_Elevation, m_Azimuth, 0.0f);
    }

    private void Fire()
    {
        if (projectilePrefab != null && m_muzzle != null)
        {
            var projectile = Instantiate(projectilePrefab, m_muzzle.position, m_muzzle.rotation);
            var projectileSpeedVector = m_muzzle.transform.TransformDirection(Vector3.up) * ProjectileSpeed;
            projectile.GetComponent<Rigidbody>().velocity = projectileSpeedVector;
        }
        if (gunSmoke && m_muzzle)
        {
            Instantiate(gunSmoke, m_muzzle.position, m_muzzle.rotation);
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
     * These assumptions allow us to use a Raycast to find where we hit in the XZ plane.
     * Then we find the projectile speed in on the XZ plane to find the speedXZ, from which we derive the time 
     * to reach that point. And with the time we can calculate y(t).
     * 
     * Note: It would also have been possible to calculate y(z) instead, so we would not have to mind about time. But the 
     * projectile equitions are generally better know in the y(t) format.
     * 
     * DrawCrosshair3D uses eliptical projectile equation: y(t) = y0 + (initialSpeed.y * t) + (1/2 * g * t * t);
     * Where y0 is the y component of the muzzle position
     *       initialSpeed.y is the y component of the projectile speed vector
     *       t is the time
     *       g is negative gravity (-9.8 m/ss)       
     */
    private void DrawCrosshair3D()
    {
        // the muzzle's 'up' is the direction it is pointing
        Vector3 projectileSpeedVector = m_muzzle.transform.TransformDirection(Vector3.up) * ProjectileSpeed;

        Ray ray = new Ray(m_muzzle.transform.position, projectileSpeedVector.normalized);
        if (m_Plane.Raycast(ray, out float intersect))
        {
            float projectileSpeedXZ = Vector3.ProjectOnPlane(projectileSpeedVector, Vector3.up).magnitude;
            var hit = ray.GetPoint(intersect);

            if (projectileSpeedXZ != 0.0f)
            {
                var deltaT = intersect / projectileSpeedXZ;
                var y = m_muzzle.transform.position.y + (projectileSpeedVector.y * deltaT) + (0.5 * Physics.gravity.y * deltaT * deltaT);
                crosshair.transform.position = new Vector3(hit.x, (float)y, hit.z - 0.03f);
            }
        }

    }
}
