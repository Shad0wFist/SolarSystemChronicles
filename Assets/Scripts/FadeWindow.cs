using UnityEngine;
using System.Collections;

public class FadeWindow : MonoBehaviour
{
    [Header("Настройки")]
    public float fadeDuration = 5f;     // Время исчезновения
    public float fadeDelay = 2f;        // Задержка перед началом
    public bool autoStart = true;       // Автоматический старт

    private Material material;
    private Color originalColor;
    private float timer = 0f;
    private bool isFading = false;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        originalColor = material.color;

        if (autoStart)
        {
            Invoke(nameof(StartFade), fadeDelay);
        }
    }

    void StartFade()
    {
        isFading = true;
        timer = 0f;
    }

    void Update()
    {
        if (isFading)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            // Меняем Alpha от 1 до 0
            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(1f, 0f, t);

            material.color = newColor;
        }
    }

    // Вызывать для ручного запуска
    public void FadeOut()
    {
        if (!isFading)
            StartFade();
    }
}