using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public float fps;
    public Text text;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
    }
    private void Update()
    {
        fps = 1.0f / Time.deltaTime;
        text.text = "" + (int)fps;
    }
}
