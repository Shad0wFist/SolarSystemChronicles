using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetButton : MonoBehaviour
{
    public string sceneName; 

    public void LoadPlanetScene()
    {
        Debug.Log("=== Попытка загрузки сцены ===");
        Debug.Log("Имя сцены в скрипте: '" + sceneName + "'");

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("ОШИБКА: У планеты не задано имя сцены!");
            return;
        }

        // Проверяем, есть ли такая сцена в билде
        bool sceneExists = false;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string buildSceneName = System.IO.Path.GetFileNameWithoutExtension(
                UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            
            if (buildSceneName == sceneName)
            {
                sceneExists = true;
                break;
            }
        }

        if (!sceneExists)
        {
            Debug.LogError($"ОШИБКА: Сцена '{sceneName}' не найдена в Build Settings! Добавьте её через File -> Build Settings.");
            return;
        }

        Debug.Log($"Загрузка сцены: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}