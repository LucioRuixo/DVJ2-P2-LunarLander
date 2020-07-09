using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;

    bool gamePaused;
    bool inputEnabled;
    bool landing;
    bool outOfFuel;

    public int minInitialX;
    public int maxInitialX;
    public int InitialY;
    public int fuelUsagePerSecond;

    public float thrustForce;
    public float rotationForce;
    public float maxLandingVelocity;
    public float maxLandingAngle;
    public float maxVelocity;
    [HideInInspector] public float height;
    [HideInInspector] public float horizontalSpeed;
    [HideInInspector] public float verticalSpeed;
    [HideInInspector] public float fuel;
    float velocity;
    float angle;
    float landingTimer;
    float landingTimerTarget;

    public Rigidbody2D rb;

    public static event Action<bool> onThrustChange;
    public static event Action<bool> onLanding;
    public static event Action onOutOfFuel;

    void OnEnable()
    {
        GameManager.onLevelSetting += SetInitialValues;
        GameManager.onLevelSetting += EnableInput;

        UIManager_Gameplay.onPauseChange += SetPause;
    }

    void Start()
    {
        gamePaused = false;
        inputEnabled = true;
        landing = false;
        outOfFuel = false;

        height = GetHeight();
        landingTimerTarget = 3f;
        fuel = model.fuelBase;
    }

    void FixedUpdate()
    {
        if (!gamePaused && inputEnabled)
        {
            if (rb)
            {
                if (Input.GetButton("Thrust") && fuel > 0)
                {
                    if (rb) rb.AddForce(transform.up * Input.GetAxis("Thrust") * thrustForce * Time.fixedDeltaTime, ForceMode2D.Force);

                    if (rb.velocity.x > maxVelocity)
                        rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
                    if (rb.velocity.y > maxVelocity)
                        rb.velocity = new Vector2(rb.velocity.x, maxVelocity);

                    if (fuel > 0f)
                    {
                        fuel -= (int)(fuelUsagePerSecond * Time.fixedDeltaTime);

                        if (fuel < 0f)
                            fuel = 0f;
                    }
                }

                if (Input.GetButton("Rotate"))
                    if (rb) rb.AddTorque(-Input.GetAxis("Rotate") * rotationForce * Time.fixedDeltaTime, ForceMode2D.Force);

                horizontalSpeed = rb.velocity.x;
                verticalSpeed = rb.velocity.y;
                velocity = rb.velocity.magnitude;
            }

            if (landing && !(velocity > maxLandingVelocity || angle > maxLandingAngle))
            {
                landingTimer += Time.fixedDeltaTime;

                if (landingTimer >= landingTimerTarget)
                {
                    inputEnabled = false;
                    landing = false;
                    landingTimer = 0f;

                    if (onLanding != null)
                        onLanding(true);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != null && collision.gameObject.tag == "Terrain")
        {
            if (velocity > maxLandingVelocity || angle > maxLandingAngle)
            {
                inputEnabled = false;
                landing = false;

                if (onLanding != null)
                    onLanding(false);
            }
            else
                landing = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != null && collision.gameObject.tag == "Terrain")
        {
            landing = false;
            landingTimer = 0f;
        }
    }

    void Update()
    {
        if (gamePaused || !inputEnabled)
            return;

        if (fuel > 0)
        {
            if (Input.GetButtonDown("Thrust") && onThrustChange != null)
                onThrustChange(true);

            if (Input.GetButtonUp("Thrust") && onThrustChange != null)
                onThrustChange(false);
        }
        else if (!outOfFuel)
        {
            outOfFuel = true;
            if (onOutOfFuel != null)
                onOutOfFuel();
        }

        height = GetHeight();
        angle = Vector2.Angle(Vector2.up, transform.up);
    }

    void OnDisable()
    {
        GameManager.onLevelSetting -= SetInitialValues;
        GameManager.onLevelSetting -= EnableInput;

        UIManager_Gameplay.onPauseChange -= SetPause;
    }

    void SetPause(bool state)
    {
        gamePaused = state;

        if (!rb)
            return;

        if (!rb.IsSleeping())
            rb.Sleep();
        else
        {
            Vector2 velocity = new Vector2(horizontalSpeed, verticalSpeed);
            rb.velocity = velocity;

            rb.WakeUp();
        }
    }

    void EnableInput()
    {
        inputEnabled = true;
    }

    void SetInitialValues()
    {
        int initialX = UnityEngine.Random.Range(minInitialX, maxInitialX);
        Vector2 initialPosition = new Vector2(initialX, InitialY);
        transform.position = initialPosition;

        transform.rotation = Quaternion.Euler(Vector2.zero);

        if (rb)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    float GetHeight()
    {
        float rayDistance = 300f;
        float heightOffset = 2.6f;

        Vector3 position = transform.position;
        position.y -= heightOffset;

        Ray ray;
        RaycastHit2D raycastHit;

        ray = new Ray(position, -Vector2.up);
        raycastHit = Physics2D.Raycast(position, -Vector2.up, rayDistance);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow);

        Debug.Log(raycastHit.collider);
        return raycastHit.distance;
    }
}