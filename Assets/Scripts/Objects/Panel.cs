using UnityEngine;
using UnityEngine.EventSystems;

public class Panel : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private Clicker clicker;
    private EnemyManager enemyManager;
    private Enemy enemy;

    private bool able = true;

    public bool swapMode;
    public bool clickMode;

    private float xySpeed;
    private float xSpeed;
    private float ySpeed;
    private float cd;

    public float lvl1;
    public float lvl2;

    void Start()
    {
        clicker = GameObject.Find("ClickerManager").GetComponent<Clicker>();
        enemyManager = GameObject.Find("ClickerManager").GetComponent<EnemyManager>();
    }
    public void EnemyGetComponent(string name)
    {
        if (name == "EnemyObj")
        {
            enemy = GameObject.Find("EnemyObj").GetComponent<Enemy>();
        }
        else if (name == "BossObj")
        {
            enemy = GameObject.Find("BossObj").GetComponent<Enemy>();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (enemyManager.clickable == true && swapMode == true)
        {
            xSpeed = eventData.delta.x > 0 ? eventData.delta.x : -eventData.delta.x;
            ySpeed = eventData.delta.y > 0 ? eventData.delta.y : -eventData.delta.y;

            if (xSpeed + ySpeed >= 60) { cd = lvl1; }
            if (xSpeed + ySpeed >= 120) { cd = lvl2; }
            else { cd = 0; }
            Hit();
        }
    }

    public void Able()
    {
        able = true;
    }

    public void Hit()
    {
        if (cd != 0 && able == true)
        {
            if (enemyManager.clickable == true && enemy)
            {
                able = false;
                Invoke(nameof(Able), cd);
                enemy.Kick();
            }
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

