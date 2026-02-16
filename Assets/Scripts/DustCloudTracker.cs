using System;
using UnityEngine;

public class DustCloudTracker : MonoBehaviour
{
    public event Action onDustCloudDestroyed;

    void OnDestroy()
    {
        onDustCloudDestroyed?.Invoke();
    }
}