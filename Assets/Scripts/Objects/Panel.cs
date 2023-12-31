using UnityEngine;
using UnityEngine.EventSystems;

public class Panel : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private Clicker clicker;
    private EnemyManager enemyManager;
    private Enemy enemy;

    public bool swapMode;
    public bool clickMode;

    private Vector2 lastPointerPosition;
    private float totalDistance = 0f;
    private const float distanceToTriggerMethod = 800f;

    void Start()
    {
        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        enemyManager = GameObject.Find("ClickerManager").GetComponent<EnemyManager>();
    }
    public void EnemyGetComponent()
    {
        enemy = FindObjectOfType<Enemy>().GetComponent<Enemy>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float deltaDistance = Vector2.Distance(lastPointerPosition, eventData.position);
        totalDistance += deltaDistance;
        lastPointerPosition = eventData.position;

        if (totalDistance >= distanceToTriggerMethod)
        {
            Hit();
            totalDistance = 0f;
        }
    }

    public void Hit()
    {
        if (enemyManager.clickable == true && enemy)
        {
            enemy.Kick();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (enemyManager.clickable == true && clickMode == true)
        {
            enemy.Kick();
        }
    }
}

