using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f; // Скорость снаряда
    public int damage = 10;  // Урон, который наносит снаряд

    private Transform target;

    // Метод для задания цели
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            // Цель уничтожена или исчезла
            Destroy(gameObject);
            return;
        }

        // Движение к цели
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Проверка на достижение цели
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            HitTarget(); // Вызов метода, когда снаряд достигает цели
        }
    }

    void HitTarget()
    {
        // Получаем компонент MoleHealth у цели, если он есть
        MoleHealth moleHealth = target.GetComponent<MoleHealth>();

        if (moleHealth != null)
        {
            // Наносим урон цели
            moleHealth.TakeDamage(damage);
        }

        // Уничтожаем снаряд
        Destroy(gameObject);
    }
}
