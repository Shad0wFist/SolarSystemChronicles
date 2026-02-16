using UnityEngine;

public class DustSpawner : MonoBehaviour
{
    public GameObject dustPrefab;        
    public float spawnInterval = 3f;     
    public float spawnRadius = 50f;      
    public int maxDustClouds = 10;       
    public float dustLifetime = 5f;      

    private float spawnTimer = 0f;
    private int currentDustCount = 0;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && currentDustCount < maxDustClouds)
        {
            SpawnDustCloud();
            spawnTimer = 0f;
        }
    }

    void SpawnDustCloud()
    {
        Vector3 spawnPosition = transform.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            Random.Range(0.5f, 3f),
            Random.Range(-spawnRadius, spawnRadius)
        );

        GameObject dustCloud = Instantiate(dustPrefab, spawnPosition, Quaternion.identity);

        Destroy(dustCloud, dustLifetime);

        currentDustCount++;

        DustCloudTracker tracker = dustCloud.AddComponent<DustCloudTracker>();
        tracker.onDustCloudDestroyed += () => currentDustCount--;
    }
}