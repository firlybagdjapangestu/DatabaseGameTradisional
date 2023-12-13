using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // GameObject yang akan diikuti oleh kamera
    public float smoothSpeed = 0.125f; // kehalusan pergerakan kamera

    public Vector3 offset; // jarak antara kamera dan target

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
