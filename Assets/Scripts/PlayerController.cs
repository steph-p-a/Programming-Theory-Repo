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


    private Transform muzzle;
    private const float projectileSpeed = 10.0f;
    private const float azimuthMin = -60.0f;
    private const float azimuthMax = 60.0f;
    private const float elevationMin = 40.0f;
    private const float elevationMax = 90.0f;

    private Vector2 m_RotationInput;
    private Vector2 m_Rotation;


    // Start is called before the first frame update
    void Start()
    {
        m_Rotation = new Vector2(transform.localEulerAngles.x, transform.localEulerAngles.y);

        muzzle = GameObject.Find("Muzzle").GetComponent<Transform>();
        if (muzzle == null)
        {
            Debug.Log("Missing Muzzle GameObject");
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
        m_Rotation.y = Mathf.Clamp(m_Rotation.y, azimuthMin, azimuthMax);
        m_Rotation.x += rotationInput.y * scaledElevationSpeed;        
        m_Rotation.x = Mathf.Clamp(m_Rotation.x, elevationMin, elevationMax);

        transform.localEulerAngles = m_Rotation;
    }

    private void Fire()
    {
        if (projectilePrefab != null && muzzle != null)
        {
            var newProjectile = Instantiate(projectilePrefab);
            newProjectile.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);
            newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.up * projectileSpeed, ForceMode.VelocityChange);
        }
    }

}
