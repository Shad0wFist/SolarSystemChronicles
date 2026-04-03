using UnityEngine;
using System.Collections;
using TMPro;

public class SceneIntroAudio : MonoBehaviour
{
    [Header("🔊 Audio Clip")]
    public AudioClip audioClip;

    [Header("📝 Subtitles")]
    public SubtitleSegment[] subtitles;

    [Header("⚙️ Settings")]
    public float startDelay = 3f;
    public float subtitleHoldTime = 2f;

    [Header("🎨 UI")]
    public GameObject subtitlePanel;
    public TextMeshProUGUI subtitleText;

    [Header("🎵 Фоновая музыка")]
    [Tooltip("Объект с фоновой музыкой")]
    public GameObject backgroundMusicObject;

    [Tooltip("Громкость музыки во время Intro (0-1)")]
    [Range(0f, 1f)]
    public float musicVolumeDuringIntro = 0.1f;

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
        if (audioClip == null)
        {
            Debug.LogWarning("⚠️ Audio Clip не назначен!");
            return;
        }

        // Приглушаем музыку
        if (backgroundMusicSource != null)
        {
            StartCoroutine(FadeMusicVolume(musicVolumeDuringIntro, 1f));
        }

        if (showDebugLogs)
            Debug.Log($"🎬 Запуск аудио последовательности: {audioClip.name}");

        StartCoroutine(PlayClipWithSubtitles());
    }

    IEnumerator PlayClipWithSubtitles()
    {
        isPlaying = true;

        audioSource.clip = audioClip;
        audioSource.Play();

        float clipStartTime = Time.time;

        if (subtitles != null && subtitles.Length > 0)
        {
            foreach (SubtitleSegment segment in subtitles)
            {
                float waitTime = segment.delay - (Time.time - clipStartTime);
                if (waitTime > 0)
                    yield return new WaitForSeconds(waitTime);

                ShowSubtitle(segment.text);

                if (showDebugLogs)
                    Debug.Log($"📝 Субтитр: {segment.text} (задержка: {segment.delay}s)");
            }
        }

        yield return new WaitForSeconds(audioClip.length);
        yield return new WaitForSeconds(subtitleHoldTime);

        HideSubtitle();

        // Возвращаем громкость музыки
        if (backgroundMusicSource != null)
        {
            StartCoroutine(FadeMusicVolume(originalMusicVolume, 1f));
        }

        isPlaying = false;

        if (showDebugLogs)
            Debug.Log("✅ Последовательность завершена!");
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