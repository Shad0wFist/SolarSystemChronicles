using UnityEngine;

public class PlayerFallReset : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("Уровень Y, ниже которого считается падением")]
    public float fallThreshold = -50f;

    [Tooltip("Куда возвращать игрока")]
    public Transform respawnPoint;

    [Tooltip("Если не назначено — использовать стартовую позицию")]
    public bool useStartPosition = true;

    private Vector3 startPosition;
    private bool isResetting = false;

    void Start()
    {
        if (useStartPosition)
            startPosition = transform.position;
    }

    void Update()
    {
        if (!isResetting && transform.position.y < fallThreshold)
        {
            StartCoroutine(ResetPlayer());
        }
    }

    System.Collections.IEnumerator ResetPlayer()
    {
        isResetting = true;

        // Возвращаем игрока
        Vector3 targetPosition = respawnPoint != null ? respawnPoint.position : startPosition;
        transform.position = targetPosition;

        // Сбрасываем скорость если есть Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Небольшая задержка перед новой проверкой
        yield return new WaitForSeconds(0.5f);

        isResetting = false;
    }
}