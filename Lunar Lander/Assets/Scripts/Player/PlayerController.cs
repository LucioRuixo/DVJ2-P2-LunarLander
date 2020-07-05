using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;

    bool gamePaused;
    bool landing;

    public int fuel;
    public int fuelUsagePerSecond;

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

    Rigidbody rb;

    public static event Action<bool> onThrustChange;
    public static event Action<bool> onLanding;

    void OnEnable()
    {
        UIManager_Gameplay.onPauseChange += SetPause;
    }

    void Start()
    {
        gamePaused = false;
        landing = false;

        height = GetHeight();
        landingTimerTarget = 3f;

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