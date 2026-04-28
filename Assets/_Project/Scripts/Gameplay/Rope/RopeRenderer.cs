using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class RopeRenderer : MonoBehaviour
{
    [SerializeField] private Transform pointA, pointB;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        pointA = transform.GetChild(0);
        pointB = transform.GetChild(1);
        lineRenderer.positionCount = 2;

        Vector3 offset = new Vector3(0.1f, 0.3f, 0);
        lineRenderer.SetPosition(0, pointA.position + offset);
        lineRenderer.SetPosition(1, pointB.position + offset);
    }
}
