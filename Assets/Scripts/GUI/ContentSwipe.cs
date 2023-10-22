using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContentSwipe : MonoBehaviour, IDragHandler
{
    public GameObject content;
    public float swipeCoef;
    public int visibleRows;
    public GameObject itemGrid;

    private float ceil;
    private float floor;

    private GridLayoutGroup gridLayout;

    void Start()
    {
        AdjustSwipeCoef();
        GetCeilValue();
    }

    void AdjustSwipeCoef()
    {
        if (Screen.height < 1000)
        {
            swipeCoef *= 4; // You can adjust this value based on your needs.
        }
        else if (Screen.height < 1920)
        {
            swipeCoef *= 2;
        }
    }

    public void GetCeilValue()
    {
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
        float newY = content.transform.localPosition.y + (eventData.delta.y * swipeCoef);
        content.transform.localPosition = new Vector2(content.transform.localPosition.x, Mathf.Clamp(newY, ceil, floor));
    }

    public void SetBack()
    {
        content.transform.localPosition = new Vector2(content.transform.localPosition.x, ceil);
    }
}