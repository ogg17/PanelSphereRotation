using UnityEngine;

namespace Physics
{
    /// <summary>
    /// Структура, в которой будет происходить рейкаст в сферу
    /// </summary>
    public readonly struct Sphere
    {
        private readonly Vector3 _center; // Центр сферы
        private readonly float _radius; // Радиус сферы

        public Sphere(Vector3 center, float radius)
        {
            _center = center;
            _radius = radius;
        }

        /// <summary>
        /// Получить центр сферы
        /// </summary>
        /// <returns></returns>
        public Vector3 GetCenter() => _center;

        /// <summary>
        /// Возвращает количество пересечений луча со сферой.
        /// <br/> Алгоримт создан решением квадратного уравнения сферы <p/>
        /// <a href="https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-sphere-intersection.html">
        /// ссылка на статью об этом уравнении</a>
        /// </summary>
        /// <param name="ray">Луч</param>
        /// <param name="distance1">Дистанция до 1 пересечения, -1 если его нет</param>
        /// <param name="distance2">Дистанция до 2 пересечения, -1 если его нет</param>
        /// <returns>int - Количество пересечений луча со сферой</returns>
        public int Raycast(Ray ray, out float distance1, out float distance2)
        {
            distance1 = -1; // Инициализируем первую дистанцию
            distance2 = -1; // Инициализируем вторую дистанцию

            Vector3 oc = ray.origin - _center; // Находим вектор проходящий от начала луча в центр сферы
            float a = Vector3.Dot(ray.direction, ray.direction); // Находим кооэфициет A квадратного уровнения
            float b = 2.0f * Vector3.Dot(oc, ray.direction); // Находим кооэфициет B квадратного уровнения
            float c = Vector3.Dot(oc, oc) - _radius * _radius; // Находим кооэфициет C квадратного уровнения
            float discriminant = b * b - 4 * a * c; // Находим дискриминант

            if (discriminant < 0)
                // Если дискриминант меньше нуля, то пересечений нет
                return 0; // Возвращаем ноль, т.е. отсутсивие пересечений

            // Считаем корень дискриминанта и сохраняем, чтобы не считать его дважды
            float sqrtDiscriminant = Mathf.Sqrt(discriminant);
            float t1 = (-b - sqrtDiscriminant) / (2.0f * a);
            float t2 = (-b + sqrtDiscriminant) / (2.0f * a);
            
            // на самом деле в большистве случаем будет 2 точки,
            // одна точка означает что луч будет касательной к сфере.
            // меньше нуля, что не пересекает сферу
            
            // В варианте с началом луча внутри сферы так же по сути 2 точки,
            // но первая точка будет "мнимой", т.е. если построить
            // обратный луч, то он и будет пересекать сферу в первой точке,
            // поэтому сохраняем только вторую дистанцию и возавращаем 1
            
            // Так же 2 пересечения будет и у луча начало которого
            // вне сферы но обратный луч пересекает сферу, в этом случае
            // или 1 или оба пересечения будут "мнимыми", соответсвенно
            // стоит возвращать 0

            // проверяем что оба перечесения не мнимые, если мнимые, то возвращаем 0
            if (t2 < 0)
                return 0;
            
            if (discriminant == 0) // Если дискриминант ноль, то пересечение только одно
            {
               // сохраняем дистанцию до этого пересечения и возращаем 1
                distance1 = t1;
                return 1;
            }
            
            // Дискриминант больше 0, значит пересечений два
            
            if (t1 < 0) 
            {
                // если 1 пересечение оказалось мнимым,
                // то сохраняем только второе и возаращаем 1
                distance1 = t2;
                return 1;
            }
            
            //сохраняем расстояния до этих пересечений и возвращаем 2
            distance1 = t1;
            distance2 = t2;

            return 2;
        }
    }
}