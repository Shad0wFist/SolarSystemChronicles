using UnityEngine;
using UnityEngine.SceneManagement;

public class TerminalManager : MonoBehaviour
{
    public static TerminalManager Instance { get; private set; }

    void Awake()
    {
        // Проверка: существует ли уже такой менеджер?
        if (Instance != null && Instance != this)
        {
            // Если существует и это не текущий объект - удаляем дубликат
            Destroy(gameObject);
            return;
        }
        else
        {
            // Если не существует - записываем этот объект как главный
            Instance = this;
            
            // Запрещаем уничтожение при загрузке сцены
            DontDestroyOnLoad(gameObject);
        }
    }

    // Метод для загрузки сцены (удобно вызывать из кнопок)
    /*public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Переход на сцену: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
    }

    // Метод для возврата на главную сцену (если нужно)
    public void LoadHomeScene(string homeSceneName)
    {
        LoadScene(homeSceneName);
    }*/
}