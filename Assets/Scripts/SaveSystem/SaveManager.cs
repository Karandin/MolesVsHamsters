using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private SaveData currentSaveData;

    private void Awake()
    {
        // Singleton pattern для сохранения единственной точки доступа к SaveManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadGame();
    }

    public void SaveGame(int highScore, int currency)
    {
        currentSaveData = new SaveData(highScore, currency);
        SaveSystem.SavePlayerData(currentSaveData);
    }

    public void LoadGame()
    {
        if (SaveSystem.SaveExists())
        {
            currentSaveData = SaveSystem.LoadPlayerData();
            ApplyLoadedData();
        }
        else
        {
            Debug.LogWarning("No save file found, starting with default data.");
            currentSaveData = new SaveData(0, 0);  // Начальные значения по умолчанию
        }
    }

    private void ApplyLoadedData()
    {
        // Применение загруженных данных к текущему состоянию игры
        GameManager.instance.SetPlayerHighScore(currentSaveData.highScore);
        GameManager.instance.SetPlayerCurrency(currentSaveData.currency);
    }

    public int GetHighScore()
    {
        return currentSaveData != null ? currentSaveData.highScore : 0;
    }

    public int GetCurrency()
    {
        return currentSaveData != null ? currentSaveData.currency : 0;
    }
}
