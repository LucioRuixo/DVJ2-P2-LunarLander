using UnityEngine;

public class Camera : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 rotationEuler;

    public Transform pivot;

    void Start()
    {
        transform.rotation = Quaternion.Euler(rotationEuler);
    }

    void Update()
    {
        if (transform.position != pivot.position + offset)
            transform.position = pivot.position + offset;

        if (transform.rotation.eulerAngles != rotationEuler)
            transform.rotation = Quaternion.Euler(rotationEuler);
    }
}