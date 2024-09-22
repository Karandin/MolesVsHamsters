using UnityEngine;

public class HamsterController : MonoBehaviour
{
    public FixedJoystick joystick;  // Ссылка на виртуальный фиксированный джойстик
    public float maxMoveSpeed = 5f; // Максимальная скорость движения хомяка
    public float accelerationTime = 0.5f; // Время для достижения полной скорости
    public float decelerationTime = 0.5f; // Время для полной остановки
    public float joystickDeadZone = 0.2f; // Порог мертвой зоны для джойстика

    private Rigidbody2D rb;
    private float currentSpeed = 0f; // Текущая скорость
    private float targetSpeed = 0f;  // Целевая скорость

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (joystick != null)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        // Получаем направление от джойстика
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Рассчитываем длину вектора направления джойстика (отклонение)
        float joystickMagnitude = new Vector2(horizontal, vertical).magnitude;

        // Если джойстик в "мертвой зоне", то целевая скорость 0 (замедление)
        if (joystickMagnitude < joystickDeadZone)
        {
            targetSpeed = 0f;
        }
        else
        {
            // Целевая скорость пропорциональна отклонению джойстика
            targetSpeed = maxMoveSpeed * joystickMagnitude;
        }

        // Плавное изменение текущей скорости до целевой скорости
        if (targetSpeed > currentSpeed)
        {
            // Ускорение
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / accelerationTime);
        }
        else if (targetSpeed < currentSpeed)
        {
            // Замедление
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / decelerationTime);
        }

        // Рассчитываем нормализованное направление движения
        Vector2 direction = new Vector2(horizontal, vertical).normalized;
        rb.velocity = direction * currentSpeed;
    }
}
