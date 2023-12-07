using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private EnemyManager enemyManager;

    private Text text;
    public float time;
    public bool timerRunning;

    private void Start()
    {
        enemyManager = GameObject.Find("ClickerManager").GetComponent<EnemyManager>();
        text = GameObject.Find("Boss(txt)").GetComponent<Text>();
    }

    void FixedUpdate()
    {
        if (timerRunning == true)
        {
            if (enemyManager.clickable)
            { time -= Time.deltaTime; text.text = time.ToString("F1") + "s."; }
            if (time <= 0)
            { enemyManager.BossFailed();
                PauseTimer();
                ClearTimer(); 
    }   }   }

    public void ClearTimer()
    {
        text.text = $"To boss.";
    }

    public void PauseTimer()
    {
        timerRunning = false;
    }

    public void StartTimer(int i)
    {
        time = i;
        text.text = $"{i}s.";
        timerRunning = true;
    }
}
