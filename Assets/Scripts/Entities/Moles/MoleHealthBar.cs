using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class MoleHealthBar : MonoBehaviour
{
    public Slider slider;               // Ссылка на UI Slider
    public TextMeshProUGUI healthText;  // Ссылка на TextMeshPro для отображения здоровья
    private Image fill;                 // Приватная ссылка на Image для изменения цвета
    public Vector3 offset = new Vector3(0, 0.5f, 0); // Смещение от объекта
    private Transform target;           // Цель, за которой следует полоска

    void Start()
    {
        // Пробуем получить компонент Image через fillRect
        if (slider.fillRect != null)
        {
            fill = slider.fillRect.GetComponent<Image>();
        }

        // Если не удалось найти fill через fillRect, пробуем найти его вручную
        if (fill == null)
        {
            fill = slider.GetComponentInChildren<Image>();
        }

        if (fill == null)
        {
            Debug.LogError("Fill Image not found! Make sure your Slider has a Fill Rect with an Image component.");
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Полоска следует за целью с учетом смещения
            transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
        }
    }

    // Устанавливаем максимальное здоровье
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        UpdateHealthBarColor(health); // Обновляем цвет при максимальном здоровье
        UpdateHealthText(health);     // Обновляем текст при изменении здоровья
    }

    // Обновляем текущее здоровье
    public void SetHealth(int health)
    {
        slider.value = health;
        UpdateHealthBarColor(health); // Обновляем цвет при изменении здоровья
        UpdateHealthText(health);     // Обновляем текст
    }

    // Устанавливаем цель для полоски
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Функция для изменения цвета в зависимости от оставшегося здоровья
    void UpdateHealthBarColor(int health)
    {
        if (fill == null) return; // Если fill не был найден, выходим из функции

        float healthPercentage = (float)health / slider.maxValue;

        if (healthPercentage >= 0.8f)
        {
            fill.color = new Color(0.0f, 1.0f, 0.0f); // Зеленый
        }
        else if (healthPercentage >= 0.6f)
        {
            fill.color = new Color(0.5f, 1.0f, 0.0f); // Светло-зеленый (Yellowish-Green)
        }
        else if (healthPercentage >= 0.4f)
        {
            fill.color = new Color(1f, 0.65f, 0f); // Оранжевый (Orange)
        }
        else
        {
            fill.color = Color.red; // Красный
        }
    }

    // Обновляем текст для отображения оставшегося здоровья
    void UpdateHealthText(int health)
    {
        if (healthText != null)
        {
            if (health >= 1000)
            {
                healthText.text = (health / 1000).ToString() + "k"; // Форматирование для 1000 и выше
            }
            else
            {
                healthText.text = health.ToString(); // Обычный вывод для здоровья меньше 1000
            }
        }
    }
}
