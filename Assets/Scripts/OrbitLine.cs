using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitLine : MonoBehaviour
{
    public Transform center; // Центр (Солнце)
    public float radius = 10f; // Радиус орбиты
    public int segments = 100; // Плавность круга

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = false;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false; // Важно: координаты локальные
        
        DrawCircle();
    }

    void DrawCircle()
    {
        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            
            // Вычисляем точку на круге
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            
            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
        }
    }
    
    // Вызывать в редакторе, если меняете радиус
    [ContextMenu("Redraw Orbit")]
    void Redraw() { DrawCircle(); }
}