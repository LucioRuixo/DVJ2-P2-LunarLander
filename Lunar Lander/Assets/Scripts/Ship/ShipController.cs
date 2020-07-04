using System;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public ShipModel model;

    float thrustForce;
    float rotationSpeed;

    Rigidbody rb;

    public static event Action<bool> onThrustChange;

    void Start()
    {
        thrustForce = model.thrustForce;
        rotationSpeed = model.rotationSpeed;

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Thrust"))
            rb.AddForce(transform.up * Input.GetAxis("Thrust") * thrustForce * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    void Update()
    {
        if (Input.GetButtonDown("Thrust") && onThrustChange != null)
            onThrustChange(true);

        if (Input.GetButton("Rotate"))
        {
            Vector3 addedRotationEuler = new Vector3(0f, 0f, Input.GetAxis("Rotate")) * rotationSpeed * Time.deltaTime;
            Vector3 newRotationEuler = transform.rotation.eulerAngles + addedRotationEuler;

            transform.rotation = Quaternion.Euler(newRotationEuler);
        }

        if (Input.GetButtonUp("Thrust") && onThrustChange != null)
            onThrustChange(false);
    }
}