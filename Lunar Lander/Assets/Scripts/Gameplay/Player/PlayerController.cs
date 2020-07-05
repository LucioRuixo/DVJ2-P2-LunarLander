using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;

    bool gamePaused;
    bool landing;

    public int minInitialX;
    public int maxInitialX;
    public int InitialY;
    public int fuel;
    public int fuelUsagePerSecond;
    int positionZ = 500;

    public float thrustForce;
    public float rotationForce;
    public float maxLandingVelocity;
    public float maxLandingAngle;
    [HideInInspector] public float height;
    [HideInInspector] public float horizontalSpeed;
    [HideInInspector] public float verticalSpeed;
    float velocity;
    float angle;
    float landingTimer;
    float landingTimerTarget;

    public Vector3 initialRotationEuler;

    Rigidbody rb;

    public static event Action<bool> onThrustChange;
    public static event Action<bool> onLanding;

    void OnEnable()
    {
        GameManager.onLevelSetting += SetInitialValues;

        UIManager_Gameplay.onPauseChange += SetPause;
    }

    void Start()
    {
        gamePaused = false;
        landing = false;

        height = GetHeight();
        landingTimerTarget = 3f;

        initialRotationEuler = new Vector3(0f, 180f, 0f);

        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 3f;
    }

    void FixedUpdate()
    {
        if (!gamePaused)
        {
            if (Input.GetButton("Thrust"))
            {
                rb.AddForce(transform.up * Input.GetAxis("Thrust") * thrustForce * Time.fixedDeltaTime, ForceMode.Acceleration);

                if (fuel > 0)
                {
                    fuel -= (int)(fuelUsagePerSecond * Time.fixedDeltaTime);

                    if (fuel < 0)
                        fuel = 0;
                }
            }

            if (Input.GetButton("Rotate"))
                rb.AddTorque(transform.forward * Input.GetAxis("Rotate") * rotationForce * Time.fixedDeltaTime, ForceMode.Force);

            if (landing && !(velocity > maxLandingVelocity || angle > maxLandingAngle))
            {
                landingTimer += Time.fixedDeltaTime;

                if (landingTimer >= landingTimerTarget)
                {
                    landing = false;

                    if (onLanding != null)
                        onLanding(true);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (velocity > maxLandingVelocity || angle > maxLandingAngle)
        {
            if (onLanding != null)
                onLanding(false);
        }
        else
            landing = true;
    }

    void OnCollisionExit(Collision collision)
    {
        landing = false;
        landingTimer = 0f;
    }

    void Update()
    {
        if (!gamePaused)
        {
            if (Input.GetButtonDown("Thrust") && onThrustChange != null)
                onThrustChange(true);

            if (Input.GetButtonUp("Thrust") && onThrustChange != null)
                onThrustChange(false);

            height = GetHeight();
            horizontalSpeed = rb.velocity.x;
            verticalSpeed = rb.velocity.y;
            velocity = rb.velocity.magnitude;
            angle = Vector3.Angle(Vector3.up, transform.up);
        }
    }

    void OnDisable()
    {
        GameManager.onLevelSetting -= SetInitialValues;

        UIManager_Gameplay.onPauseChange -= SetPause;
    }

    void SetPause(bool state)
    {
        gamePaused = state;

        if (!rb.IsSleeping())
            rb.Sleep();
        else
        {
            Vector3 velocity = new Vector3(horizontalSpeed, verticalSpeed, 0f);
            rb.velocity = velocity;

            rb.WakeUp();
        }
    }

    void SetInitialValues()
    {
        int initialX = UnityEngine.Random.Range(minInitialX, maxInitialX);
        Vector3 initialPosition = new Vector3(initialX, InitialY, positionZ);
        transform.position = initialPosition;

        transform.rotation = Quaternion.Euler(initialRotationEuler);
    }

    float GetHeight()
    {
        float rayDistance = 500f;

        Ray ray;
        RaycastHit raycastHit;

        ray = new Ray(transform.position, -Vector3.up);
        Physics.Raycast(ray, out raycastHit, rayDistance);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow);

        return raycastHit.distance;
    }
}