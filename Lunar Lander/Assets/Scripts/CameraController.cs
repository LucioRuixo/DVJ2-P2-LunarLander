using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float offsetY;
    public float zoomMultiplier = 1f;
    public float minZoomDistance = 20f;
    public float maxZoomDistance = 1000f;

    public Vector3 rotationEuler;
    Vector3 offset;

    public Transform pivot;
    public PlayerController pivotController;

    void Start()
    {
        transform.rotation = Quaternion.Euler(rotationEuler);

        offset = Vector3.zero;
        offset.y = offsetY;
        offset.z = Mathf.Clamp(pivotController.height * zoomMultiplier * -1f, -maxZoomDistance, -minZoomDistance);

        transform.position = pivot.position + offset;
    }

    void Update()
    {
        if (transform.rotation.eulerAngles != rotationEuler)
            transform.rotation = Quaternion.Euler(rotationEuler);

        if (offset.y != offsetY)
            offset.y = offsetY;

        offset.z = Mathf.Clamp(pivotController.height * zoomMultiplier * -1f, -maxZoomDistance,  -minZoomDistance);
        transform.position = pivot.position + offset;
    }
}