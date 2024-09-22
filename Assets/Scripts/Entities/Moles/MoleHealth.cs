using UnityEngine;
using UnityEngine.Events;

public class MoleHealth : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;

    public MoleHealthBar healthBarPrefab;
    private MoleHealthBar healthBar;

    public UnityEvent OnMoleDeath;  // Событие смерти крота

    private MoleStealCookieController moleStealCookieController;  // Обновляем ссылку на новый скрипт

    void Start()
    {
        moleStealCookieController = GetComponent<MoleStealCookieController>();  // Получаем компонент MoleStealCookieController

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in the scene!");
            return;
        }

        healthBar = Instantiate(healthBarPrefab, transform);
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetTarget(transform);

        healthBar.transform.SetParent(canvas.transform, false);

        if (OnMoleDeath == null)
            OnMoleDeath = new UnityEvent();
    }

    public void SetHealth(int health)
    {
        maxHealth = health;
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Вызываем событие смерти, чтобы другие менеджеры могли реагировать
        OnMoleDeath.Invoke();

        if (moleStealCookieController != null)
        {
            moleStealCookieController.OnMoleDeath();  // Логика кражи печенья
        }

        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }
}
