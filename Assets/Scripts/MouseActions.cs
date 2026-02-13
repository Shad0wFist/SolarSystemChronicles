using UnityEngine;
using UnityEngine.Events;

public class MouseActions : MonoBehaviour
{
    public UnityEvent _down;
    public UnityEvent _enter;
    public UnityEvent _exit;

    private void OnMouseDown()
    {
        _down?.Invoke();
    }
    private void OnMouseEnter()
    {
        _enter?.Invoke();
    }
    private void OnMouseExit()
    {
        _exit?.Invoke();
    }
}
