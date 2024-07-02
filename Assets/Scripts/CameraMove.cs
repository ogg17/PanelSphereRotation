using UnityEngine;

/// <summary>
/// Скрипт движения камеры с помощью клавиатуры
/// </summary>
public class CameraMove : MonoBehaviour
{
    [Tooltip("Скорость движения камеры"), Range(1, 20), SerializeField] private float speed = 5f;

    private Vector3 _direction; // Кеш напрвления, чтобы не создавать его в Update

    private void Update()
    {
        _direction.Set(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) // Нажата W двигаемся вперед
        {
            _direction.x = transform.forward.x;
            _direction.z = transform.forward.z;
        }

        if (Input.GetKey(KeyCode.S)) // Нажата S двигаемся назад
        {
            _direction.x -= transform.forward.x;
            _direction.z -= transform.forward.z;
        }

        if (Input.GetKey(KeyCode.A)) // Нажата A двигаемся влево
        {
            _direction.x -= transform.right.x;
            _direction.z -= transform.right.z;
        }

        if (Input.GetKey(KeyCode.D)) // Нажата D двигаемся вправо
        {
            _direction.x += transform.right.x;
            _direction.z += transform.right.z;
        }

        _direction.Normalize();
        transform.position += Time.deltaTime * speed * _direction;
    }
}