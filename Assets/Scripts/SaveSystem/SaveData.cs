[System.Serializable]
public class SaveData
{
    public int highScore;
    public int currency;

    public SaveData(int highScore, int currency)
    {
        this.highScore = highScore;
        this.currency = currency;
    }
}
