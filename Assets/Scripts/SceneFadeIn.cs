using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("Время исчезновения черного экрана (секунды)")]
    public float fadeDuration = 2f;

    [Tooltip("Задержка перед началом исчезновения")]
    public float startDelay = 0.5f;

    [Tooltip("Черный экран (панель с Image)")]
    public Image FadePanel;

    void Start()
    {
        // Если панель не назначена — ищем в сцене
        if (FadePanel == null)
            FadePanel = GetComponent<Image>();

        // Начинаем исчезновение
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // Ждем задержку
        yield return new WaitForSeconds(startDelay);

        // Плавно меняем прозрачность от 1 до 0
        float elapsed = 0f;
        Color originalColor = FadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            // Меняем только Alpha, цвет оставляем черным
            FadePanel.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                Mathf.Lerp(1f, 0f, t)
            );

            yield return null;
        }

        // Полностью прозрачный в конце
        FadePanel.color = new Color(0, 0, 0, 0);

        // Опционально: отключить объект после завершения
        FadePanel.gameObject.SetActive(false);
    }
}