using UnityEngine;


public class CameraControl : MonoBehaviour
{
    public float speed = 2.0f; // Скорость движения камеры
    public Transform target; // Цель, за которой будет следовать камера

    private void Awake()
    {
        // Если цель не задана, ищем первый объект типа Character в сцене
        if (!target)
        {
            target = FindFirstObjectByType<Character>().transform;
        }
    }

    private void LateUpdate()
    {
        // Определяем новую позицию камеры
        Vector3 position = target.position;
        position.z = -10.0f; // Устанавливаем z-координату камеры

        // Плавно перемещаем камеру к новой позиции
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}