using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContentSwipe : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public GameObject content;
    public float swipeCoef;
    public float decelerateCoef = 1;
    public int visibleRows;
    public GameObject itemGrid;

    private float ceil;
    private float floor;
    private Vector2 delta;
    private bool decelerate;
    [SerializeField] private float decelerateSpeed;

    private GridLayoutGroup gridLayout;

    void Start()
    {
        AdjustSwipeCoef();
        GetCeilValue();
    }

    void AdjustSwipeCoef()
    {
        swipeCoef = swipeCoef * 2000 / Screen.height;
    }

    public void GetCeilValue()
    {
        SetBack();
        if (itemGrid != null)
        {
            gridLayout = itemGrid.GetComponent<GridLayoutGroup>();
            int rowCount = Mathf.CeilToInt((float)itemGrid.transform.childCount / gridLayout.constraintCount);
            if (rowCount > visibleRows)
            {
                ceil = content.transform.localPosition.y;
                floor = (gridLayout.cellSize.y + gridLayout.spacing.y) * (rowCount - visibleRows) - ceil;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        delta = eventData.delta;
        decelerateSpeed = delta.y;
        float newY = content.transform.localPosition.y + (delta.y * swipeCoef);
        content.transform.localPosition = new Vector2(content.transform.localPosition.x, Mathf.Clamp(newY, ceil, floor));
    }

    public void SetBack()
    {
        content.transform.localPosition = new Vector2(content.transform.localPosition.x, ceil);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        decelerate = true;
    }
    private void Update()
    {
        if (decelerate)
        {
            if (decelerateSpeed > 0)
            {
                decelerateSpeed -= Time.deltaTime * decelerateCoef;
            }
            else
            {
                decelerateSpeed += Time.deltaTime * decelerateCoef;
            }

            float newY = content.transform.localPosition.y + (decelerateSpeed * swipeCoef);
            content.transform.localPosition = new Vector2(content.transform.localPosition.x, Mathf.Clamp(newY, ceil, floor));

            if (Mathf.Abs(decelerateSpeed) < 1f)
            {
                decelerate = false;
                decelerateSpeed = 0;
            }
        }
    }
}