using UnityEngine;

public class GoldReward : MonoBehaviour
{
    public int rewardAmount = 5; // Количество золота за смерть крота

    private void Start()
    {
        MoleHealth moleHealth = GetComponent<MoleHealth>();
        if (moleHealth != null)
        {
            moleHealth.OnMoleDeath.AddListener(RewardPlayer);
        }
    }

    private void RewardPlayer()
    {
        GoldManager.instance.AddGold(rewardAmount); // Начисляем золото через GoldManager
        // Обновляем стоимость на кнопке после получения золота
        FindObjectOfType<TurretManager>().UpdateButtonCostText();
    }
}
