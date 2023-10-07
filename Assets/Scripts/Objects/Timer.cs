using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private EnemyManager enemyManager;

    private Slider slider;
    public float time;
    public bool timerRunning;

    private void Start()
    {
        enemyManager = GameObject.Find("ClickerManager").GetComponent<EnemyManager>();
        slider = GameObject.Find("Timer(sld)").GetComponent<Slider>();
    }

    void FixedUpdate()
    {
        if (timerRunning == true)
        {
            if (enemyManager.clickable)
            { time -= Time.deltaTime; slider.value = time; }
            if (time <= 0)
            { enemyManager.BossFailed();
                TakeResult();
                PauseTimer();
                ClearTimer(); 
    }   }   }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void TakeResult()
    {
        Debug.Log(slider.maxValue - time);
        slider.value = slider.maxValue;
    }
    public void ClearTimer()
    {
        slider.value = slider.maxValue;
    }

    public void PauseTimer()
    {
        timerRunning = false;
    }

    public void StartTimer(int i)
    {
        time = i;
        slider.value = time;
        slider.maxValue = time;
        timerRunning = true;
    }
}
