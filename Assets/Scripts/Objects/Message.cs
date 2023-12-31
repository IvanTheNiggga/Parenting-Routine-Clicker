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

        //���������� ����� ����������� � ������� ���� � ������� �������
        text.text = message;
        objectMovement.MoveTo(new Vector2(0, 0), 0.1f, 0.1f, false);
        soundManager.PlayBruhSound();

        Invoke(nameof(GoBack), secondsToRead);
    }

    private void GoBack()
    {
        objectMovement.MoveTo(new Vector2(900, 0), 0.1f, 0.1f, false);
    }
}
