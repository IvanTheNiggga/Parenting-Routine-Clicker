using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public Text text;
    private ObjectMovement objectMovement;

    private void Start()
    {
        objectMovement = GetComponent<ObjectMovement>();
    }

    public void SendMessage(string message, int secondsToRead)
    {
        CancelInvoke();
        text.text = message;
        objectMovement.MoveTo(new Vector2(0, 475), 0.1f, 0.1f, false);
        Invoke(nameof(GoBack), secondsToRead);
    }

    private void GoBack()
    {
        objectMovement.MoveTo(new Vector2(0, 900), 0.1f, 0.1f, false);
    }
}
