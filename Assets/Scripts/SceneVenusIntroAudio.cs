using UnityEngine;
using System.Collections;
using TMPro;

public class SceneVenusIntroAudio : MonoBehaviour
{
    [Header("🔊 Audio Clips")]
    public AudioClip firstClip;
    public AudioClip secondClip;
    public AudioClip thirdClip;

    [Header("📝 Subtitles - Первый клип")]
    public SubtitleSegment[] firstSubtitles;

    [Header("📝 Subtitles - Второй клип")]
    public SubtitleSegment[] secondSubtitles;

    [Header("📝 Subtitles - Третий клип")]
    public SubtitleSegment[] thirdSubtitles;

    [Header("⚙️ Settings")]
    [Tooltip("Задержка между клипами (секунды)")]
    public float delayBetweenClips = 1f;

    [Tooltip("Задержка перед началом последовательности")]
    public float startDelay = 3f;

    [Tooltip("Сколько показывать последний субтитр после окончания")]
    public float subtitleHoldTime = 2f;

    [Header("🎨 UI")]
    public GameObject subtitlePanel;
    public TextMeshProUGUI subtitleText;

    [Header("🎵 Фоновая музыка")]
    [Tooltip("Объект с фоновой музыкой")]
    public GameObject backgroundMusicObject;

    [Tooltip("Громкость музыки во время последовательности (0-1)")]
    [Range(0f, 1f)]
    public float musicVolumeDuringSequence = 0.1f;

    [Tooltip("Время плавного изменения громкости (секунды)")]
    public float fadeDuration = 1f;

    [Header("🔍 Debug")]
    public bool showDebugLogs = true;

    private AudioSource audioSource;
    private AudioSource backgroundMusicSource;
    private float originalMusicVolume = 1f;
    private bool isPlaying = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;

        if (subtitlePanel != null)
            subtitlePanel.SetActive(false);

        // Находим фоновую музыку
        if (backgroundMusicObject != null)
        {
            backgroundMusicSource = backgroundMusicObject.GetComponent<AudioSource>();
            if (backgroundMusicSource != null)
                originalMusicVolume = backgroundMusicSource.volume;
        }

        Invoke(nameof(StartAudioSequence), startDelay);
    }

    void StartAudioSequence()
    {
        // Проверяем, что есть хотя бы один клип
        if (firstClip == null && secondClip == null && thirdClip == null)
        {
            Debug.LogWarning("⚠️ Ни один Audio Clip не назначен!");
            return;
        }

        // Приглушаем музыку
        if (backgroundMusicSource != null)
        {
            StartCoroutine(FadeMusicVolume(musicVolumeDuringSequence, fadeDuration));
        }

        if (showDebugLogs)
            Debug.Log("🎬 Запуск аудио последовательности (3 клипа)");

        StartCoroutine(PlayAudioSequence());
    }

    IEnumerator PlayAudioSequence()
    {
        isPlaying = true;

        // === ПЕРВЫЙ КЛИП ===
        if (firstClip != null)
        {
            if (showDebugLogs)
                Debug.Log($"🔊 Клип 1: {firstClip.name}");

            yield return StartCoroutine(PlayClipWithSubtitles(firstClip, firstSubtitles));
        }

        yield return new WaitForSeconds(delayBetweenClips);

        // === ВТОРОЙ КЛИП ===
        if (secondClip != null)
        {
            if (showDebugLogs)
                Debug.Log($"🔊 Клип 2: {secondClip.name}");

            yield return StartCoroutine(PlayClipWithSubtitles(secondClip, secondSubtitles));
        }

        yield return new WaitForSeconds(delayBetweenClips);

        // === ТРЕТИЙ КЛИП ===
        if (thirdClip != null)
        {
            if (showDebugLogs)
                Debug.Log($"🔊 Клип 3: {thirdClip.name}");

            yield return StartCoroutine(PlayClipWithSubtitles(thirdClip, thirdSubtitles));
        }

        // Держим последний субтитр
        yield return new WaitForSeconds(subtitleHoldTime);

        // Скрываем субтитры
        HideSubtitle();

        // Возвращаем громкость музыки
        if (backgroundMusicSource != null)
        {
            StartCoroutine(FadeMusicVolume(originalMusicVolume, fadeDuration));
        }

        isPlaying = false;

        if (showDebugLogs)
            Debug.Log("✅ Последовательность завершена!");
    }

    IEnumerator PlayClipWithSubtitles(AudioClip clip, SubtitleSegment[] subtitles)
    {
        // Воспроизводим аудио
        audioSource.clip = clip;
        audioSource.Play();

        float clipStartTime = Time.time;

        // Показываем субтитры по расписанию
        if (subtitles != null && subtitles.Length > 0)
        {
            foreach (SubtitleSegment segment in subtitles)
            {
                // Ждём нужное время от начала клипа
                float waitTime = segment.delay - (Time.time - clipStartTime);
                if (waitTime > 0)
                    yield return new WaitForSeconds(waitTime);

                // Показываем текст
                ShowSubtitle(segment.text);

                if (showDebugLogs)
                    Debug.Log($"📝 Субтитр: {segment.text} (задержка: {segment.delay}s)");
            }
        }

        // Ждём окончания клипа
        yield return new WaitForSeconds(clip.length);
    }

    IEnumerator FadeMusicVolume(float targetVolume, float duration)
    {
        if (backgroundMusicSource == null) yield break;

        float startVolume = backgroundMusicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            backgroundMusicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        backgroundMusicSource.volume = targetVolume;
    }

    void ShowSubtitle(string text)
    {
        if (subtitlePanel != null)
            subtitlePanel.SetActive(true);

        if (subtitleText != null && !string.IsNullOrEmpty(text))
            subtitleText.text = text;
    }

    void HideSubtitle()
    {
        if (subtitlePanel != null)
            subtitlePanel.SetActive(false);

        if (subtitleText != null)
            subtitleText.text = "";
    }

    public void StopSequence()
    {
        StopAllCoroutines();
        audioSource.Stop();
        HideSubtitle();

        if (backgroundMusicSource != null)
            backgroundMusicSource.volume = originalMusicVolume;

        isPlaying = false;
    }
}