using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public Text text;
    private ObjectMovement objectMovement;
    private SoundManager soundManager;

    private void Start()
    {
        objectMovement = GetComponent<ObjectMovement>();
        soundManager = FindObjectOfType<SoundManager>().GetComponent<SoundManager>();
    }

    public void SendMessage(string message, int secondsToRead)
    {
        CancelInvoke();

        //Определяем текст уведомления и выводим окно в видимую область
        text.text = message;
        objectMovement.xMoveTo(0, 0.1f, 0.1f, false);
        soundManager.PlayBruhSound();

        Invoke(nameof(GoBack), secondsToRead);
    }

    private void GoBack()
    {
        objectMovement.xMoveTo(900, 0.1f, 0.1f, false);
    }
}
