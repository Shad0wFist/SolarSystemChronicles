using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionController : MonoBehaviour
{
    [Header("Настройки перехода")]
    [Tooltip("Время затемнения окна (секунды)")]
    public float windowFadeDuration = 3f;

    [Tooltip("Время затемнения экрана (секунды)")]
    public float screenFadeDuration = 2f;

    [Tooltip("Задержка перед началом перехода")]
    public float startDelay = 30f;

    [Tooltip("Название следующей сцены")]
    public string nextSceneName = "Venus";

    [Header("Ссылки")]
    [Tooltip("Окно корабля (Renderer с материалом)")]
    public Renderer windowRenderer;

    [Tooltip("Чёрная панель на весь экран (Image)")]
    public Image fadePanel;

    private bool isTransitioning = false;
    private Color windowOriginalColor;

    void Start()
    {
        if (windowRenderer != null)
        {
            // Сохраняем исходный цвет (чёрный, непрозрачный)
            windowOriginalColor = windowRenderer.material.color;
            windowOriginalColor.a = 1f;
        }

        Invoke(nameof(StartTransition), startDelay);
    }

    public void StartTransition()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        // ✅ ОТКЛЮЧАЕМ FadeWindow, чтобы он не мешал!
        FadeWindow fadeScript = windowRenderer?.GetComponent<FadeWindow>();
        if (fadeScript != null)
        {
            fadeScript.enabled = false;
        }

        // === ЭТАП 1: Затемняем окно корабля ===
        yield return StartCoroutine(FadeWindowToBlack());

        yield return new WaitForSeconds(0.3f);

        // === ЭТАП 2: Затемняем экран игрока ===
        yield return StartCoroutine(FadeScreenToBlack());

        // === ЭТАП 3: Загружаем сцену ===
        LoadNextScene();
    }

    private IEnumerator FadeWindowToBlack()
    {
        if (windowRenderer != null)
        {
            Material mat = windowRenderer.material;
            Color startColor = mat.color;  // Текущий (прозрачный)
            Color endColor = windowOriginalColor;  // Исходный чёрный
            endColor.a = 1f;

            float elapsed = 0f;

            while (elapsed < windowFadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / windowFadeDuration;

                // Плавно меняем цвет от прозрачного к чёрному
                mat.color = Color.Lerp(startColor, endColor, t);

                yield return null;
            }

            // Фиксируем финальное состояние
            mat.color = endColor;
        }

        yield return null;
    }

    private IEnumerator FadeScreenToBlack()
    {
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            float elapsed = 0f;

            while (elapsed < screenFadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / screenFadeDuration;

                // Меняем Alpha от 0 до 1 (чёрный)
                fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(0f, 1f, t));

                yield return null;
            }

            fadePanel.color = new Color(0, 0, 0, 1);
        }

        yield return null;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void TriggerTransition()
    {
        StartTransition();
    }
}