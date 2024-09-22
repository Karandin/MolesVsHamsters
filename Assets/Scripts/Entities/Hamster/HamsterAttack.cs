using UnityEngine;

public class HamsterAttack : MonoBehaviour
{
    public float range = 5f;           // Радиус атаки хомяка
    public float fireRate = 0.5f;      // Скорострельность хомяка (выстрелов в секунду)
    public GameObject projectilePrefab; // Префаб снаряда
    public Transform firePoint;        // Точка выстрела хомяка

    private float fireCountdown = 0f;
    private Transform target;

    void Update()
    {
        UpdateTarget();

        if (target != null)
        {
            // Поворот firePoint в сторону цели, без изменения самой позиции хомяка
            Vector2 direction = target.position - firePoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0f, 0f, angle); // Поворот точки выстрела к цели

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    void UpdateTarget()
    {
        // Поиск ближайшего врага в радиусе атаки хомяка
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = collider.transform;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        // Создание снаряда и отправка к цели
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Seek(target);
        }
    }
}
