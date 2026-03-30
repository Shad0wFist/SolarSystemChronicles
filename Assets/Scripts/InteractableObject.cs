using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Настройки курсора")]
    public bool changeCursorOnHover = true; // Менять ли курсор при наведении

    private bool isHovered = false;

    void Start()
    {
        // Важно: у объекта должен быть Collider (Box или Sphere), иначе мышь его не увидит!
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning("У объекта " + name + " нет Collider! Добавьте его.");
        }
    }

    void OnMouseEnter()
    {
        isHovered = true;
        
        // Визуальная обратная связь через курсор (опционально)
        if (changeCursorOnHover)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        
        Debug.Log("Наведение на: " + name);
    }

    void OnMouseExit()
    {
        isHovered = false;
        
        // Возвращаем курсор в исходное состояние
        if (changeCursorOnHover)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    void OnMouseDown()
    {
        if (isHovered)
        {
            Debug.Log("Клик по объекту: " + name);
            
            // Если это Планета -> загружаем сцену
            PlanetButton planetBtn = GetComponent<PlanetButton>();
            if (planetBtn != null) 
            {
                planetBtn.LoadPlanetScene();
            }

            // Если это Экран -> запускаем меню
            TerminalScreen terminal = GetComponent<TerminalScreen>();
            if (terminal != null) 
            {
                terminal.OpenMenu();
            }
        }
    }
}