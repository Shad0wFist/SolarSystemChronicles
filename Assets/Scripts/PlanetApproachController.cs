using UnityEngine;
using System.Collections;

public class PlanetApproachController : MonoBehaviour
{
    [Header("Настройки движения")]
    public Transform shipTarget;
    public float approachSpeed = 50f;
    public float stopDistance = 20f;

    [Header("Атмосфера")]
    public Renderer atmosphereRenderer;
    public float startAlpha = 0f;
    public float endAlpha = 1f;
    public float alphaStartDistance = 100f;
    public float alphaFullDistance = 30f;

    [Header("Переход на сцену")]
    public bool autoTransition = true;
    public string nextSceneName = "VenusLand";
    public float transitionDelay = 2f;

    private bool isApproaching = false;
    private bool hasArrived = false;
    private Material atmosphereMaterial;

    void Start()
    {
        // Сохраняем ссылку на материал
        if (atmosphereRenderer != null)
        {
            atmosphereMaterial = atmosphereRenderer.material;
            SetAlpha(startAlpha);
        }
    }

    void Update()
    {
        if (!isApproaching || hasArrived) return;

        MovePlanet();
        UpdateAtmosphereAlpha();
        CheckArrival();
    }

    public void StartApproach()
    {
        if (isApproaching) return;
        isApproaching = true;
        hasArrived = false;
        Debug.Log("Начало приближения к планете...");
    }

    private void MovePlanet()
    {
        if (shipTarget == null) return;
        Vector3 direction = (shipTarget.position - transform.position).normalized;
        transform.position += direction * approachSpeed * Time.deltaTime;
    }

    // ✅ ИСПРАВЛЕНО: Правильная установка Alpha
    private void UpdateAtmosphereAlpha()
    {
        if (atmosphereMaterial == null || shipTarget == null) return;

        float distance = Vector3.Distance(transform.position, shipTarget.position);
        float t = Mathf.InverseLerp(alphaStartDistance, alphaFullDistance, distance);
        float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, t);

        SetAlpha(currentAlpha);
    }

    // ✅ УНИВЕРСАЛЬНЫЙ метод установки Alpha
    private void SetAlpha(float alpha)
    {
        if (atmosphereMaterial == null) return;

        // Пробуем разные варианты (для Shader Graph)
        if (atmosphereMaterial.HasProperty("_Alpha"))
        {
            atmosphereMaterial.SetFloat("_Alpha", alpha);
        }

        // Всегда обновляем color.a (это работает всегда!)
        Color color = atmosphereMaterial.color;
        color.a = alpha;
        atmosphereMaterial.color = color;

        // Для некоторых Shader Graph нужно _BaseColor
        if (atmosphereMaterial.HasProperty("_BaseColor"))
        {
            Color baseColor = atmosphereMaterial.GetColor("_BaseColor");
            baseColor.a = alpha;
            atmosphereMaterial.SetColor("_BaseColor", baseColor);
        }
    }

    private void CheckArrival()
    {
        if (shipTarget == null) return;

        float distance = Vector3.Distance(transform.position, shipTarget.position);

        if (distance <= stopDistance && !hasArrived)
        {
            hasArrived = true;

            // Фиксируем позицию
            Vector3 direction = (shipTarget.position - transform.position).normalized;
            transform.position = shipTarget.position - (direction * stopDistance);

            // Фиксируем Alpha = 1
            SetAlpha(endAlpha);

            Debug.Log("Планета прибыла!");

            if (autoTransition)
            {
                Invoke(nameof(TriggerSceneTransition), transitionDelay);
            }
        }
    }

    private void TriggerSceneTransition()
    {
        SceneTransitionOnArrival transition = FindObjectOfType<SceneTransitionOnArrival>();

        if (transition != null)
        {
            transition.StartTransition();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }

    public void ResetApproach()
    {
        isApproaching = false;
        hasArrived = false;

        if (atmosphereMaterial != null)
        {
            SetAlpha(startAlpha);
        }
    }
}