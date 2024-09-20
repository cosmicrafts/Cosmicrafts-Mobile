using UnityEngine;
using System.Collections;


public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float delay = 0.01f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        StartCoroutine(SmoothFollow());
    }

    IEnumerator SmoothFollow()
    {
        yield return new WaitForSeconds(delay);

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
