using UnityEngine;
using TMPro;

public class TurretManager : MonoBehaviour
{
    public GameObject turretPrefab;  // Префаб турели
    public TextMeshProUGUI buttonCostText; // Текст для отображения стоимости на кнопке
    private GameObject selectedTurret; // Выбранная турель для размещения
    private bool isPlacingTurret = false; // Флаг режима размещения
    private int turretCost = 10; // Начальная стоимость турели (целое число)
    private const float costIncreasePercentage = 0.2f; // Увеличение стоимости на 20%

    private void Start()
    {
        UpdateButtonCostText();

        // Подписка на событие смерти кротов
        MoleHealth[] allMoles = FindObjectsOfType<MoleHealth>();
        foreach (MoleHealth mole in allMoles)
        {
            mole.OnMoleDeath.AddListener(UpdateButtonCostText);
        }
    }

    // Метод для кнопки, активирует режим размещения турели
    public void OnSelectTurret()
    {
        if (GoldManager.instance.GetGoldAmount() >= turretCost)
        {
            isPlacingTurret = true;
            selectedTurret = Instantiate(turretPrefab);

            if (selectedTurret.GetComponent<Collider2D>() != null)
            {
                selectedTurret.GetComponent<Collider2D>().enabled = false;
            }
            if (selectedTurret.GetComponent<Rigidbody2D>() != null)
            {
                Destroy(selectedTurret.GetComponent<Rigidbody2D>());
            }
        }
        else
        {
            Debug.Log("Недостаточно золота для размещения турели!");
        }
    }

    void Update()
    {
        if (isPlacingTurret)
        {
            FollowMouse();

            if (Input.GetMouseButtonDown(0))
            {
                PlaceTurret();
            }
        }
    }

    void FollowMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        selectedTurret.transform.position = mousePosition;
    }

    void PlaceTurret()
    {
        if (CanPlaceTurret(selectedTurret.transform.position))
        {
            GoldManager.instance.AddGold(-turretCost); // Тратим золото

            if (selectedTurret.GetComponent<Collider2D>() != null)
            {
                selectedTurret.GetComponent<Collider2D>().enabled = true;
            }

            Rigidbody2D rb = selectedTurret.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;

            isPlacingTurret = false;

            TurretAttack turretAttack = selectedTurret.GetComponent<TurretAttack>();
            if (turretAttack != null)
            {
                turretAttack.isPlaced = true;
            }

            // Увеличиваем стоимость турели
            turretCost = Mathf.CeilToInt(turretCost * (1 + costIncreasePercentage));

            // Обновляем текст на кнопке с новой стоимостью
            UpdateButtonCostText();

            selectedTurret = null;
        }
    }

    // Обновляем текст на кнопке с текущей стоимостью турели и изменяем цвет в зависимости от количества золота
    public void UpdateButtonCostText()
    {
        buttonCostText.text = turretCost.ToString();

        if (GoldManager.instance.GetGoldAmount() >= turretCost)
        {
            buttonCostText.color = Color.green; // Достаточно золота или ровно столько
        }
        else
        {
            buttonCostText.color = Color.red; // Недостаточно золота
        }
    }

    bool CanPlaceTurret(Vector3 position)
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(position, 0.5f);
        if (hitCollider != null && hitCollider.CompareTag("Base"))
        {
            Debug.Log("Нельзя ставить турель на базу!");
            return false;
        }
        return true;
    }
}
