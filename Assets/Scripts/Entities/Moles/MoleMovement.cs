using UnityEngine;

public class MoleMovement : MonoBehaviour
{
    public Transform hamsterBase;
    public Transform moleBase;
    public float speed = 2f;
    public int waypointsCount = 3;

    private Vector3[] waypoints;
    private int currentWaypointIndex = 0;
    private bool movingToHamsterBase = true;
    private bool returningToMoleBase = false;

    private MoleStealCookieController moleStealCookieController;  // Ссылка на компонент кражи печенья

 void Start()
{
    // Игнорируем коллизии между объектами в слое Moles (предположим, что Moles — это слой с индексом 8)
    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Moles"), LayerMask.NameToLayer("Moles"));
    
    moleStealCookieController = GetComponent<MoleStealCookieController>();  // Получаем компонент MoleStealCookieController
    GenerateWaypoints();
}


    void Update()
    {
        MoveTowardsTarget();
    }

    void GenerateWaypoints()
    {
        waypoints = new Vector3[waypointsCount + 1];

        Vector3 lastPosition = transform.position;

        for (int i = 0; i < waypointsCount; i++)
        {
            float randomX = Random.Range(lastPosition.x + 1f, lastPosition.x + 5f);
            float randomY = Random.Range(-4f, 4f);
            waypoints[i] = new Vector3(randomX, randomY, 0);
            lastPosition = waypoints[i];
        }

        waypoints[waypointsCount] = hamsterBase.position;
    }

    void MoveTowardsTarget()
    {
        if (returningToMoleBase)
        {
            Vector3 target = moleBase.position;
            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target) < 0.2f)
            {
                moleStealCookieController.DeliverCookie();  // Крот доставил кусочек на свою базу
                Destroy(gameObject);  // Уничтожаем крота
            }
        }
        else if (currentWaypointIndex < waypoints.Length)
        {
            Vector3 target = waypoints[currentWaypointIndex];
            Vector3 direction = (target - transform.position).normalized;

            if (direction.x < 0)
            {
                direction.x = 0;
            }

            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target) < 0.2f)
            {
                currentWaypointIndex++;
            }
        }
        else if (movingToHamsterBase)
        {
            movingToHamsterBase = false;
            returningToMoleBase = true;

            // Крадём печенье, когда достигаем базы хомяков
            moleStealCookieController.StealCookie();
        }
    }
}
