using Scripts.Core.Lifetime;
using UnityEngine;
using UnityEngine.Profiling;

namespace Scripts.Game.Butterflies
{
    public class ButterflyFlight : MonoBehaviour
    {
        [Header("Основные настройки")]
        public float baseSpeed = 1.5f;
        public float speedVariation = 0.5f;

        [Header("Крылья")]
        public float wingFlapFrequency = 5f;
        public float wingFlapAmplitude = 30f;

        [Header("Траектория полета")]
        public float directionChangeFrequency = 2f;
        public float maxTurnAngle = 45f;
        public float altitudeChangeSpeed = 0.5f;

        [Header("Случайные движения")]
        public float randomMovementFrequency = 3f;
        public float randomMovementStrength = 0.3f;

        private Vector3 currentDirection;
        private float currentSpeed;
        private float wingFlapTimer;
        private float directionChangeTimer;
        private float randomMovementTimer;
        private Vector3 randomMovementOffset;
        private float targetAltitude;

        void Start()
        {
            InitializeMovement();
            targetAltitude = transform.position.y;
        }

        void InitializeMovement()
        {
            // Случайное начальное направление
            currentDirection = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-0.5f, 0.5f),
                0
            ).normalized;

            currentSpeed = baseSpeed + Random.Range(-speedVariation, speedVariation);
        }

        void Update()
        {
            Profiler.BeginSample("ButterflyFlight.Update");
            if (ApplicationState.IsPaused.CurrentValue)
            {
                Profiler.EndSample();
                return;
            }
            UpdateTimers();
            UpdateDirection();
            UpdateWingFlap();
            UpdateRandomMovement();
            ApplyMovement();
            Profiler.EndSample();
        }

        void UpdateTimers()
        {
            wingFlapTimer += Time.deltaTime * wingFlapFrequency;
            directionChangeTimer += Time.deltaTime;
            randomMovementTimer += Time.deltaTime;
        }

        void UpdateDirection()
        {
            if (directionChangeTimer >= 1f / directionChangeFrequency)
            {
                // Плавное изменение направления
                Vector3 newDirection = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-0.3f, 0.5f), // Бабочки склонны летать немного вверх
                    0
                ).normalized;

                currentDirection = Vector3.Slerp(currentDirection, newDirection, 0.1f);

                // Изменение скорости
                currentSpeed = baseSpeed + Random.Range(-speedVariation, speedVariation);

                directionChangeTimer = 0f;
            }

            // Периодическое изменение высоты
            if (Random.Range(0f, 1f) < 0.02f)
            {
                targetAltitude = transform.position.y + Random.Range(-2f, 2f);
            }
        }

        void UpdateWingFlap()
        {
            // Анимация взмахов крыльев через вращение
            float wingFlap = Mathf.Sin(wingFlapTimer * Mathf.PI * 2) * wingFlapAmplitude;
            transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x,
                transform.localEulerAngles.y,
                wingFlap
            );
        }

        void UpdateRandomMovement()
        {
            if (randomMovementTimer >= 1f / randomMovementFrequency)
            {
                randomMovementOffset = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    0
                ) * randomMovementStrength;

                randomMovementTimer = 0f;
            }
        }

        void ApplyMovement()
        {
            // Плавное изменение высоты
            float currentY = transform.position.y;
            float newY = Mathf.Lerp(currentY, targetAltitude, Time.deltaTime * altitudeChangeSpeed);

            // Основное движение + случайные отклонения
            Vector3 movement = (currentDirection * currentSpeed + randomMovementOffset) * Time.deltaTime;
            movement.y = newY - currentY;

            transform.position += movement;
        }

        // Для отладки - визуализация направления в редакторе
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, currentDirection * 2f);
        }
    }
}