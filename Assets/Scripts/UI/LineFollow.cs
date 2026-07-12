using Unity.VisualScripting;
using UnityEngine;

public class LineFollow : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private void Awake()
    {
        lineRenderer.positionCount = 2;
        Vector2 start = new Vector2(startPoint.position.x, startPoint.position.y);
        Vector2 end = new Vector2(endPoint.position.x, endPoint.position.y);
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
