using UnityEngine;
using TMPro; // Подключаем TextMeshPro

public class GemManager : MonoBehaviour
{
    public static GemManager Instance;

    public TextMeshProUGUI gemText; // Используем TextMeshProUGUI вместо обычного Text
    private int gemAmount = 0; // Стартовое количество гемов (можно менять)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Гемы сохраняются между сессиями
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateGemDisplay();
    }

    public void AddGems(int amount)
    {
        gemAmount += amount;
        UpdateGemDisplay();
    }

    public bool SpendGems(int amount)
    {
        if (gemAmount >= amount)
        {
            gemAmount -= amount;
            UpdateGemDisplay();
            return true;
        }
        return false;
    }

    private void UpdateGemDisplay()
    {
        gemText.text = "Gems: " + gemAmount.ToString();
    }

    public int GetGemAmount()
    {
        return gemAmount;
    }
}
