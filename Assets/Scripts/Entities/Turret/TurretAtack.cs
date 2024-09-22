using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    public float range = 5f;           // Радиус атаки
    public float fireRate = 1f;        // Скорострельность (выстрелов в секунду)
    public GameObject projectilePrefab; // Префаб снаряда
    public Transform firePoint;        // Точка выстрела

    private float fireCountdown = 0f;
    private Transform target;
    
    // Добавляем переменную для проверки установки
    public bool isPlaced = false;   // Турель не установлена по умолчанию

    void Update()
    {
        // Если турель не установлена, не выполняем атаку
        if (!isPlaced)
        {
            return;
        }

        UpdateTarget();

        if (target != null)
        {
            // Поворот турели к цели (опционально)
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

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
        // Поиск ближайшего врага в радиусе
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
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Рисуем радиус атаки в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
