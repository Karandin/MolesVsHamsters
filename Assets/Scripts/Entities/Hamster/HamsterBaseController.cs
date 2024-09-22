using UnityEngine;
using TMPro;  // Если используешь TextMeshPro

public class HamsterBaseController : MonoBehaviour
{
    public int totalCookiePieces = 5;  // Общее количество кусочков печенья
    private int currentCookiePieces;

    public TextMeshProUGUI cookieHealthText;  // Ссылка на объект текста

    private void Start()
    {
        currentCookiePieces = totalCookiePieces;
        UpdateCookieDisplay();  // Обновляем текст при старте
    }

    // Уменьшаем количество кусочков печенья
    public void LoseCookiePiece()
    {
        if (currentCookiePieces > 0)
        {
            currentCookiePieces--;
            UpdateCookieDisplay();  // Обновляем текст
        }

        // Убираем вызов EndGame, так как мы хотим завершать игру только при доставке кусочка на базу кротов
    }

    // Возвращаем кусочек печенья
    public void RestoreCookiePiece()
    {
        if (currentCookiePieces < totalCookiePieces)
        {
            currentCookiePieces++;
            UpdateCookieDisplay();  // Обновляем текст
        }
    }

    // Обновление текста здоровья
    private void UpdateCookieDisplay()
    {
        cookieHealthText.text = "Cookie Health: " + currentCookiePieces.ToString();
    }
}
