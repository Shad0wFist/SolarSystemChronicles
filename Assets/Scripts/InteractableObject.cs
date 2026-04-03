using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    private bool m_ChangeCursorOnHover = true;

    [SerializeField]
    private float m_HapticIntensity = 0.5f;
    [SerializeField]
    private float m_HapticDuration = 0.1f;

    // Эти методы вызываются XRI автоматически при событиях луча
    public void OnXRHoverEnter(HoverEnterEventArgs args)
    {
        Debug.Log("VR Луч наведен на: " + name);
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor controllerInteractor)
        {
            controllerInteractor.SendHapticImpulse(m_HapticIntensity, m_HapticDuration);
        }

        HandleHoverEffect(true);
    }

    public void OnXRHoverExit(HoverExitEventArgs args)
    {
        Debug.Log("VR Луч покинул: " + name);

        HandleHoverEffect(false);
    }

    public void OnXRSelectEntered(SelectEnterEventArgs args)
    {
        // Select в XRI по умолчанию привязан к кнопке Trigger или Grip
        ExecuteInteraction();
    }

    private void ExecuteInteraction()
    {
        Debug.Log("Взаимодействие с: " + name);

        // Логика для планеты
        if (TryGetComponent(out PlanetButton planetBtn))
        {
            planetBtn.LoadPlanetScene();
        }

        // Логика для терминала
        if (TryGetComponent(out TerminalScreen terminal))
        {
            terminal.OpenMenu();
        }
    }

    private void HandleHoverEffect(bool isEntering)
    {
        if (m_ChangeCursorOnHover)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    // Оставляем для поддержки мыши (ПК-тесты)
    private void OnMouseEnter() => HandleHoverEffect(true);
    private void OnMouseExit() => HandleHoverEffect(false);
    private void OnMouseDown() => ExecuteInteraction();

}