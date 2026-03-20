using UnityEngine;

public enum RotationAxis { X, Y, Z, Custom }

public class PlanetRotation : MonoBehaviour
{
    [Header("Настройки вращения")]
    public float rotationSpeed = 10f;

    [Header("Ось вращения")]
    public RotationAxis selectedAxis = RotationAxis.Y;
    public Vector3 customAxis = Vector3.up;

    [Header("Направление")]
    public bool clockwise = false; 

    void Update()
    {
        // Определяем ось
        Vector3 axis = selectedAxis switch
        {
            RotationAxis.X => Vector3.right,
            RotationAxis.Y => Vector3.up,
            RotationAxis.Z => Vector3.forward,
            RotationAxis.Custom => customAxis,
            _ => Vector3.up
        };

        // Определяем направление
        float direction = clockwise ? -1f : 1f;

        // Вращаем
        transform.Rotate(axis * rotationSpeed * direction * Time.deltaTime, Space.Self);
    }
}