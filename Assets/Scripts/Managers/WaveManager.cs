using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // Префаб врага (крота)
    public Transform spawnPoint;    // Точка спавна врагов (например, база кротов)
    public int numberOfEnemies = 5; // Количество врагов в первой волне
    public float healthMultiplier = 1.2f; // Множитель для увеличения здоровья
    public int baseHealth = 35; // Базовое здоровье первой волны
    public float timeBetweenWaves = 30f; // Время между волнами (в секундах)

    private int waveNumber = 1;

    void Start()
    {
        // Запускаем корутину для спавна волн
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true) // Бесконечный цикл для спавна волн
        {
            SpawnWave(); // Спавним волну
            yield return new WaitForSeconds(timeBetweenWaves); // Ждем 30 секунд до следующей волны
        }
    }

    public void SpawnWave()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Спавним врагов с небольшим смещением, чтобы они не появлялись в одном месте
            Vector3 spawnPosition = spawnPoint.position + new Vector3(i * 0.5f, 0, 0); // Смещение по X
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Рассчитываем здоровье для этой волны и передаем его кроту
            int waveHealth = Mathf.CeilToInt(baseHealth * Mathf.Pow(healthMultiplier, waveNumber - 1));
            MoleHealth moleHealth = newEnemy.GetComponent<MoleHealth>();

            if (moleHealth != null)
            {
                moleHealth.SetHealth(waveHealth);

                // Подписываемся на событие смерти крота для начисления очков и золота
                GameManager.instance.SubscribeToMoleDeath(moleHealth);
                GoldManager.instance.SubscribeToMoleDeath(moleHealth);
            }
        }

        waveNumber++; // Увеличиваем номер волны
    }
}
