using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [Tooltip("Скорость вращения камеры"), Range(0.01f, 0.5f), SerializeField] private float speed = 0.075f;
    [Tooltip("Верхняя граница"), SerializeField] private float upBorder = 85;
    [Tooltip("Нижняя граница"), SerializeField] private float downBorder = -85;

    private Vector3 _lastMousePosition; // Поле для хранения последней позиции курсора мыши
    private Vector3 _newRotation; // Кеш для нового вращения, чтобы не создавать вектор каждый кард
    
    private void Update()
    {
        // Находим дельту позиции курсора мыши
        var delta = Input.mousePosition - _lastMousePosition;

        if (Input.GetMouseButton(1))
        {
            _newRotation.y = transform.eulerAngles.y + delta.x * speed; // Вычисляем угол Y
            float eulerAngleX = transform.eulerAngles.x - delta.y * speed; // Вычисляем угол X
            
            // Проверяем новый угол X на превышение границы
            if(eulerAngleX < upBorder && eulerAngleX > downBorder 
               || eulerAngleX <= 360 + upBorder && eulerAngleX > 360 + downBorder)
                _newRotation.x = eulerAngleX; // Если не превышена, то присваиваем новый угол
            
            transform.rotation = Quaternion.Euler(_newRotation); // Присваиваем новое вращение трансформу
        }
        
        // сохраняем позицию курсора для вычисления дельты в следующем кадре
        _lastMousePosition = Input.mousePosition;
    }
}