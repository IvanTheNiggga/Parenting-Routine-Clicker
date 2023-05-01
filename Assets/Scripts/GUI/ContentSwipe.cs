using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContentSwipe : MonoBehaviour, IDragHandler
{
    public GameObject content;

    public float swipeCoef;

    public PointerEventData eventData;

    public int visibleRows;
    public float floor;
    public float ceil;
    public GameObject itemGrid;

    public void OnDrag(PointerEventData eventData)
    {
        if (content.transform.localPosition.y > ceil)
        {
            content.transform.localPosition = new Vector2(content.transform.localPosition.x, content.transform.localPosition.y + (eventData.delta.y * swipeCoef));
        }
        else
        {
            content.transform.localPosition = new Vector2(content.transform.localPosition.x, ceil);
        }
        if (content.transform.localPosition.y < floor)
        {
            content.transform.localPosition = new Vector2(content.transform.localPosition.x, content.transform.localPosition.y + (eventData.delta.y * swipeCoef));
        }
        else
        {
            content.transform.localPosition = new Vector2(content.transform.localPosition.x, floor);
        }
    }

    void Start()
    {
        if(Screen.height < 1920)
        {
            swipeCoef *= 2;
        }
        if (Screen.height < 1000)
        {
            swipeCoef *= 2;
        }
        ceil = content.transform.localPosition.y;
        Invoke(nameof(CheckFloor), 1f);
    }

    public void CheckFloor()
    {
        if (itemGrid != null)
        {
            GridLayoutGroup gl = itemGrid.GetComponent<GridLayoutGroup>();
            int rows = 0;
            int noVisibleChildCount = itemGrid.transform.childCount;
            while (noVisibleChildCount > 0)
            {
                noVisibleChildCount -= gl.constraintCount;
                rows++;
            }
            if (rows > visibleRows)
            {
                floor = ((gl.cellSize.y + gl.spacing.y) * (rows - visibleRows)) - ceil;
    }   }   }


    public void SetBack()
    {
        content.transform.localPosition = new Vector2(content.transform.localPosition.x, ceil);
    }
}
