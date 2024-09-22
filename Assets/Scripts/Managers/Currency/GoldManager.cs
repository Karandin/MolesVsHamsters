using UnityEngine;
using TMPro; // Подключаем TextMeshPro

public class GoldManager : MonoBehaviour
{
    public static GoldManager instance;

    public TextMeshProUGUI goldText; // Используем TextMeshProUGUI вместо обычного Text
    private int goldAmount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        UpdateGoldDisplay();
    }

    public void AddGold(int amount)
    {
        goldAmount += amount;
        UpdateGoldDisplay();
    }

    public void ResetGold()
    {
        goldAmount = 0;
        UpdateGoldDisplay();
    }

    private void UpdateGoldDisplay()
    {
        goldText.text = "Gold: " + goldAmount.ToString();
    }

    public int GetGoldAmount()
    {
        return goldAmount;
    }

    public void SubscribeToMoleDeath(MoleHealth mole)
    {
        // Подписываемся на событие смерти крота
        mole.OnMoleDeath.AddListener(() => AddGold(10));  // Начисляем золото
    }
}
