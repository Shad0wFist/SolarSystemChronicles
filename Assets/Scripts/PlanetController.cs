using UnityEngine;

public class PlanetController : MonoBehaviour
{
    [Header("Настройки орбиты")]
    public Transform sunTransform; // Ссылка на Солнце (или центр)
    public float orbitSpeed = 10f; // Скорость вращения вокруг солнца

    [Header("Настройки вращения")]
    public float rotationSpeed = 50f; // Скорость вращения вокруг своей оси

    void Update()
    {
        // 1. Вращение вокруг своей оси
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // 2. Вращение вокруг Солнца
        // Вариант А: Если вы используете иерархию (Пустой объект-родитель в центре)
        // Тогда просто вращаем родителя этого объекта. 
        // Но если скрипт висит на самой планете, проще использовать RotateAround:
        
        if (sunTransform != null)
        {
            transform.RotateAround(sunTransform.position, Vector3.up, orbitSpeed * Time.deltaTime);
        }
    }
}