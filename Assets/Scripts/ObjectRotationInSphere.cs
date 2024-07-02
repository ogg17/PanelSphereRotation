using Physics;
using UnityEngine;

/// <summary>
/// Вращает объект по внутренней поверхности сферы
/// </summary>
public class ObjectRotationInSphere : MonoBehaviour
{
    [Tooltip("Центр сферы, по внутренней поверхности которой будет происходить перемещение")]
    [SerializeField] private Transform center;
    
    [Tooltip("Радиус сферы, по внутренней поверхности которой будет происходить перемещение")]
    [SerializeField] private float radius = 1f;

    private Sphere _sphere; // Сама структура сферы для рейкаста
    private Camera _main; // Кеш камеры, чтобы не получать компонент камеры в Update

    private void Start()
    {
        _main = Camera.main; // Запоняем кеш компонента камеры из которой будет исходить луч
        
        _sphere = new Sphere(center.position, radius); // Создаем структуру Sphere - сферу
    }
    
    private void Update()
    {
        // Если нажата ЛКМ пускаем луч из положения курсора мыши на камере
        if (Input.GetMouseButton(0))
            Raycast(_main.ScreenPointToRay(Input.mousePosition));
    }

    /// <summary>
    /// Кастит луч и изменяет положение к последней точке
    /// персечения с лучом и поворачивает к центру сферы
    /// </summary>
    /// <param name="ray">Луч</param>
    public void Raycast(Ray ray)
    {
        // Пускаем рейкаст и получаем количесво пересечений и расстояния до этих пересечений
        var count = _sphere.Raycast(ray, out var distance1, out var distance2);
        
        // Проверяем сколько пересечений
        switch (count)
        {
            case 0: // Если 0, то выходим из метода
                return;
            case 1: // Если 1, то меняем позицию к этой точке
            {
                transform.position = ray.GetPoint(distance1);
                break;
            }
            case 2: // Если 2, то меняем позицию ко второй точке
            {
                transform.position = ray.GetPoint(distance2);
                break;
            }
            default: // Если больше 2, то выводим варнинг и выходим из метода
                Debug.LogWarning("Что-то не так, у луча и сферы не может быть больше 2 пересечений!");
                return;
        }

        // Поворачиваемся к центру сферы
        transform.LookAt(_sphere.GetCenter());
        // Переварачиваем т.к. канвас отрисовывается на задней стороне объекта
        // А LookAt поворачивает объект передом к центру
        transform.forward *= -1; 
    }
}