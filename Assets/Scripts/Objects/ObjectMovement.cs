using UnityEngine;
using UnityEngine.Events;

public class ObjectMovement : MonoBehaviour
{
    public UnityEvent onTargetReached;

    private float Speed;
    private float FinalRemainder;

    private bool move;
    public bool DoEvent;

    Vector2 Target;
    public Vector2 StartPos;

    void Start()
    {
        if (StartPos == new Vector2(1, 1))
        {
            StartPos = transform.localPosition;
        }
        else
        {
            transform.localPosition = StartPos;
        }
    }

    public void MoveTo(Vector2 target, float speed, float finalRemainder, bool doEvent)
    {
        Target = target;
        Speed = speed;
        DoEvent = doEvent;

        FinalRemainder = finalRemainder;

        move = true;
    }
    private void FixedUpdate()
    {
        if (move)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, Target, Speed);
            
            if (Vector2.Distance(transform.localPosition, Target) <= FinalRemainder)
            {
                move = false;

                if (DoEvent == true)
                {
                    onTargetReached.Invoke();
                }
                transform.localPosition = Target;
            }
        }
    }
}
