using UnityEngine;

public class MoleStealCookieController : MonoBehaviour
{
    private bool hasStolenCookie = false;  // Указывает, украл ли крот кусочек печенья
    private HamsterBaseController hamsterBaseController;  // Ссылка на базу хомяков

    private void Start()
    {
        // Находим объект HamsterBaseController в сцене
        hamsterBaseController = FindObjectOfType<HamsterBaseController>();

        if (hamsterBaseController == null)
        {
            Debug.LogError("HamsterBaseController не найден на сцене!");
        }
    }

    // Вызывается, когда крот достигает базы хомяков и крадёт кусочек печенья
    public void StealCookie()
    {
        if (!hasStolenCookie && hamsterBaseController != null && hamsterBaseController.totalCookiePieces > 0)
        {
            hamsterBaseController.LoseCookiePiece();  // Крадём кусочек
            hasStolenCookie = true;  // Обновляем флаг, что крот украл печенье
            Debug.Log("Крот украл кусочек печенья!");
        }
    }

    // Вызывается при смерти крота, возвращаем украденный кусочек печенья
    public void OnMoleDeath()
    {
        if (hasStolenCookie && hamsterBaseController != null)
        {
            hamsterBaseController.RestoreCookiePiece();  // Возвращаем кусочек печенья
            hasStolenCookie = false;  // Сбрасываем флаг
            Debug.Log("Кусочек печенья возвращён на базу хомяков!");
        }
    }

    // Вызывается, когда крот успешно доставляет печенье на свою базу
    public void DeliverCookie()
    {
        if (hasStolenCookie)
        {
            Debug.Log("Крот доставил кусочек печенья на свою базу!");
            hasStolenCookie = false;  // Сбрасываем флаг, так как кусочек доставлен

            // Увеличиваем счётчик доставленных кусочков
            GameManager.instance.AddDeliveredCookie();

            // Проверяем, завершилась ли игра
            GameManager.instance.CheckEndGame();
        }
    }
}
