using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LightningSpawnController : MonoBehaviour
{
    [System.Serializable]
    private struct LightningItem
    {
        public GameObject GameObject;
        public ParticleSystem RootParticle;
        public AudioSource Audio;
        public float VisualDuration;
    }

    [SerializeField]
    private GameObject m_LightningPrefab;

    [SerializeField]
    private int m_PoolSize = 12;

    [SerializeField]
    private float m_MinWait = 0.5f;

    [SerializeField]
    private float m_MaxWait = 3f;

    [SerializeField]
    private AudioClip[] m_ThunderClips;

    [SerializeField]
    private Transform m_PlayerCamera;

    private List<LightningItem> m_Pool = new List<LightningItem>();

    private BoxCollider m_SpawnZone;

    private void Awake()
    {
        m_SpawnZone = GetComponent<BoxCollider>();

        for (int i = 0; i < m_PoolSize; i++)
        {
            GameObject obj = Instantiate(m_LightningPrefab, transform);
            obj.SetActive(false);

            LightningItem item = new LightningItem();
            item.GameObject = obj;
            item.RootParticle = obj.GetComponent<ParticleSystem>();
            item.Audio = obj.GetComponent<AudioSource>();

            var mainModule = item.RootParticle.main;
            item.VisualDuration = mainModule.duration;

            ParticleSystemRenderer renderer = obj.GetComponent<ParticleSystemRenderer>();

            if (renderer != null)
            {
                renderer.sortingFudge = -500f;
            }

            m_Pool.Add(item);
        }
    }

    private void Start()
    {
        StartCoroutine(LightningRoutine());
    }

    private IEnumerator LightningRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(m_MinWait, m_MaxWait);
            yield return new WaitForSeconds(waitTime);

            ActivateLightning();
        }
    }

    private void ActivateLightning()
    {
        for (int i = 0; i < m_Pool.Count; i++)
        {
            if (m_Pool[i].GameObject.activeSelf == false)
            {
                LightningItem bolt = m_Pool[i];

                bolt.GameObject.transform.position = GetRandomPointInBounds();
                bolt.GameObject.SetActive(true);

                bolt.RootParticle.Clear(true);
                bolt.RootParticle.Play(true);

                float totalLifeTime = bolt.VisualDuration;

                if (m_ThunderClips.Length > 0 && bolt.Audio != null)
                {
                    AudioClip randomClip = m_ThunderClips[Random.Range(0, m_ThunderClips.Length)];
                    bolt.Audio.clip = randomClip;

                    bolt.Audio.pitch = Random.Range(0.75f, 1.25f);

                    float distance = Vector3.Distance(bolt.GameObject.transform.position, m_PlayerCamera.position);
                    float soundDelay = distance / 343f;

                    bolt.Audio.PlayDelayed(soundDelay);

                    float audioLifeTime = soundDelay + randomClip.length;

                    if (audioLifeTime > totalLifeTime)
                    {
                        totalLifeTime = audioLifeTime;
                    }
                }

                StartCoroutine(DeactivateAfterTime(bolt.GameObject, totalLifeTime));

                return;
            }
        }
    }

    private Vector3 GetRandomPointInBounds()
    {
        Vector3 min = m_SpawnZone.bounds.min;
        Vector3 max = m_SpawnZone.bounds.max;

        return new Vector3(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }

    private IEnumerator DeactivateAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        obj.SetActive(false);
    }

}