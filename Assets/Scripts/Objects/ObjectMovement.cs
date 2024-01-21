using UnityEngine;
using UnityEngine.Events;

public class ObjectMovement : MonoBehaviour
{
    public UnityEvent onTargetReached;

    private float Speed;
    private float FinalRemainder;

    private bool move;
    private bool move_x;
    public bool DoEvent;

    float Target;

    public Vector2 StartPos;

    void Start()
    {
        if (StartPos.x == 1)
        {
            StartPos.x = transform.localPosition.x;
        }
        if (StartPos.y == 1)
        {
            StartPos.y = transform.localPosition.y;
        }
        transform.localPosition = StartPos;
    }

    public void yMoveTo(float target, float speed, float finalRemainder, bool doEvent)
    {
        Target = target;
        Speed = speed;
        DoEvent = doEvent;

        FinalRemainder = finalRemainder;

        move = true;
        move_x = false;
    }
    public void xMoveTo(float target, float speed, float finalRemainder, bool doEvent)
    {
        Target = target;
        Speed = speed;
        DoEvent = doEvent;

        FinalRemainder = finalRemainder;

        move = true;
        move_x = true;
    }
    private void FixedUpdate()
    {
        if (move)
        {
            Vector2 pos = transform.localPosition;
            if (move_x)
            {
                transform.localPosition = new Vector2(Mathf.Lerp(pos.x, Target, Speed), pos.y);
                if (Mathf.Abs(pos.x - Target) <= FinalRemainder)
                {
                    move = false;

                    if (DoEvent == true)
                    {
                        onTargetReached.Invoke();
                    }
                    transform.localPosition = new Vector2(Target, pos.y);
                }
            }
            else
            {
                transform.localPosition = new Vector2(pos.x, Mathf.Lerp(pos.y, Target, Speed));
                if (Mathf.Abs(pos.y - Target) <= FinalRemainder)
                {
                    move = false;

                    if (DoEvent == true)
                    {
                        onTargetReached.Invoke();
                    }
                    transform.localPosition = new Vector2(pos.x, Target);
                }
            }
        }
    }
}