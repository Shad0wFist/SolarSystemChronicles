using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionOnArrival : MonoBehaviour
{
    [Header("Настройки перехода")]
    [Tooltip("Время затемнения экрана (секунды)")]
    public float fadeDuration = 2f;

    [Tooltip("Название следующей сцены")]
    public string nextSceneName = "VenusLand";

    [Header("Ссылки")]
    [Tooltip("Чёрная панель на весь экран (Image)")]
    public Image fadePanel;

    private bool isTransitioning = false;

    void Start()
    {
        // Инициализация панели
        if (fadePanel != null)
        {
            fadePanel.color = new Color(0, 0, 0, 0);
            fadePanel.gameObject.SetActive(false);
        }
    }

    // Запуск перехода (вызывается из PlanetApproachController)
    public void StartTransition()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        // === ЭТАП 1: Затемнение экрана ===
        yield return StartCoroutine(FadeScreenToBlack());

        // Небольшая пауза
        yield return new WaitForSeconds(0.5f);

        // === ЭТАП 2: Загрузка сцены ===
        LoadNextScene();
    }

    private IEnumerator FadeScreenToBlack()
    {
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;

                fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(0f, 1f, t));

                yield return null;
            }

            fadePanel.color = new Color(0, 0, 0, 1);
        }

        yield return null;
    }

    private void LoadNextScene()
    {
        Debug.Log("Загрузка сцены: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

    // Для ручного запуска
    public void TriggerTransition()
    {
        StartTransition();
    }
}