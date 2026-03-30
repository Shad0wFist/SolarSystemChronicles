using UnityEngine;
using System.Collections;

public class TerminalScreen : MonoBehaviour
{
    [System.Serializable]
    public class PlanetData
    {
        public GameObject planetPrefab; // Префаб планеты (сфера с текстурой)
        public Vector3 targetPosition;  // Куда она должна прилететь (в воздухе)
        public string planetName;       // Имя для сцены
    }

    public PlanetData[] planets; // Массив данных для всех планет
    public Transform spawnPoint; // Точка, откуда вылетают планеты (центр экрана)

    private bool isOpen = false;

    public void OpenMenu()
    {
        if (isOpen) return;
        isOpen = true;

        foreach (var data in planets)
        {
            // Создаем планету
            GameObject newPlanet = Instantiate(data.planetPrefab, spawnPoint.position, Quaternion.identity);
            
            // Добавляем скрипт клика на новую планету
            PlanetButton btn = newPlanet.AddComponent<PlanetButton>();
            btn.sceneName = data.planetName; 
            
            // Добавляем обводку (не забудьте создать префаб с обводкой или добавить её кодом)
            // Для простоты предположим, что у префаба уже есть InteractableObject
            
            // Запускаем анимацию полета
            StartCoroutine(MoveToTarget(newPlanet.transform, data.targetPosition, 1.5f));
        }
    }

    // Простая анимация полета по дуге
    IEnumerator MoveToTarget(Transform obj, Vector3 target, float duration)
    {
        Vector3 startPos = obj.position;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // от 0 до 1

            // Линейное перемещение
            Vector3 currentPos = Vector3.Lerp(startPos, target, t);
            
            // Добавляем высоту для дуги (синусоида)
            currentPos.y += Mathf.Sin(t * Mathf.PI) * 2f; // 2f - высота подъема

            obj.position = currentPos;
            yield return null;
        }
        obj.position = target;
    }
}