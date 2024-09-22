using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameStateEnum currentGameState = GameStateEnum.Playing;  // Текущее состояние игры
    public GameObject gameOverPanel;  // Панель "Конец игры"
    
    public int totalCookiePieces = 5;  // Общее количество кусочков
    private int deliveredCookies = 0;  // Счётчик доставленных кусочков

    public int score = 0;  // Счёт
    public TextMeshProUGUI scoreText;  // TextMeshProUGUI для отображения очков
    public TextMeshProUGUI highScoreText;  // TextMeshProUGUI для отображения лучшего рекорда

    private int highScore = 0;  // Переменная для хранения лучшего рекорда

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Убедимся, что объект сохраняется между сценами
        }
        else
        {
            Destroy(gameObject);
        }
        
        LoadHighScore();  // Загружаем рекорд при запуске игры
    }

    private void Start()
    {
        UpdateScoreText();
        UpdateHighScoreText();  // Обновляем отображение рекорда на старте
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore.ToString();
        }
    }

    // Метод для подписки на событие смерти крота
    public void SubscribeToMoleDeath(MoleHealth mole)
    {
        mole.OnMoleDeath.AddListener(() => AddScore(1));
    }

    // Метод для добавления доставленного печенья
    public void AddDeliveredCookie()
    {
        deliveredCookies++;
        Debug.Log("Доставлено кусочков печенья: " + deliveredCookies);
        CheckEndGame(); // Проверяем, закончилась ли игра
    }

    // Проверка на завершение игры
    public void CheckEndGame()
    {
        if (deliveredCookies >= totalCookiePieces)
        {
            EndGame();
        }
    }

    // Метод завершения игры
    public void EndGame()
    {
        if (currentGameState != GameStateEnum.GameOver)
        {
            currentGameState = GameStateEnum.GameOver;
            Debug.Log("Игра окончена!");

            // Сравнение текущего счета с рекордом
            if (score > highScore)
            {
                highScore = score;  // Обновляем рекорд
                SaveHighScore();    // Сохраняем новый рекорд
            }

            UpdateHighScoreText();  // Обновляем текст рекорда

            // Останавливаем все игровые процессы
            PauseGame();

            // Показываем панель конца игры
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }
    }

    // Пауза игры — заморозка всех процессов
    public void PauseGame()
    {
        Time.timeScale = 0;  // Замораживаем время
    }

    // Возобновление игры
    public void ResumeGame()
    {
        Time.timeScale = 1;  // Возвращаем нормальное время
        currentGameState = GameStateEnum.Playing;
        gameOverPanel.SetActive(false);  // Скрываем панель "Конец игры"
    }

    // Сохранение рекорда
    private void SaveHighScore()
    {
        SaveData data = new SaveData(highScore, 0);  // Сохраняем только рекорд, пока без валюты
        SaveSystem.SavePlayerData(data);  // Используем SaveSystem для сохранения
    }

    // Загрузка рекорда
    private void LoadHighScore()
    {
        SaveData data = SaveSystem.LoadPlayerData();
        if (data != null)
        {
            highScore = data.highScore;  // Загружаем рекорд
        }
    }

    // Установка рекорда (используется для загрузки или обновления)
    public void SetPlayerHighScore(int highScore)
    {
        this.highScore = highScore;  // Присваиваем значение рекорда
        UpdateHighScoreText();  // Обновляем текст на экране
    }

    //todo Здесь добавь логику для установки валюты игрока
    public void SetPlayerCurrency(int currency)
    {
        // Логика для работы с валютой (если потребуется в будущем)
    }
}
