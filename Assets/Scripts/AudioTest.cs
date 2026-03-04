using UnityEngine;
using System.Collections;
using TMPro;

[System.Serializable]
public class SubtitleSegment
{
    public string text;
    public float delay; // Задержка перед показом (в секундах)
}

public class AudioTest : MonoBehaviour
{
    [Header("🔊 Audio Clips")]
    public AudioClip firstClip;   // morsetest
    public AudioClip secondClip;  // test

    [Header("📝 Subtitles - Первый клип")]
    public SubtitleSegment[] firstSubtitles;

    [Header("📝 Subtitles - Второй клип")]
    public SubtitleSegment[] secondSubtitles;

    [Header("⚙️ Settings")]
    public float delayBetweenClips = 1f;
    public float defaultSubtitleDuration = 3f; // Сколько показывать текст по умолчанию

    [Header("🎨 UI")]
    public GameObject subtitlePanel;
    public TextMeshProUGUI subtitleText;

    [Header("🔍 Debug")]
    public bool hasBeenTriggered = false;
    public bool showDebugLogs = true;

    private AudioSource audioSource;
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
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered || isPlaying) return;

        if (other.CompareTag("Player") || other.gameObject.name.Contains("Player"))
        {
            if (showDebugLogs)
                Debug.Log("🎯 Игрок вошёл в зону триггера!");

            StartCoroutine(PlayAudioSequence());
        }
    }

    IEnumerator PlayAudioSequence()
    {
        hasBeenTriggered = true;
        isPlaying = true;

        // === ПЕРВЫЙ КЛИП ===
        if (firstClip != null)
        {
            if (showDebugLogs)
                Debug.Log($"🔊 Воспроизводим первый клип: {firstClip.name}");

            yield return StartCoroutine(PlayClipWithSubtitles(firstClip, firstSubtitles));
        }

        yield return new WaitForSeconds(delayBetweenClips);

        // === ВТОРОЙ КЛИП ===
        if (secondClip != null)
        {
            if (showDebugLogs)
                Debug.Log($"🔊 Воспроизводим второй клип: {secondClip.name}");

            yield return StartCoroutine(PlayClipWithSubtitles(secondClip, secondSubtitles));
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

        // Скрываем субтитры
        HideSubtitle();
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
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            if (col is BoxCollider box)
            {
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawCube(box.center, box.size);
            }
        }
    }
}
