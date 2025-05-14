using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dragged = eventData.pointerDrag;
        if (dragged == null) return;

        Transform draggedParent = dragged.GetComponent<BlockDragHandler>()?.originalParent;

        // 현재 슬롯에 이미 블록이 들어 있다면
        if (transform.childCount > 0)
        {
            Transform currentBlock = transform.GetChild(0);

            // 현재 블록의 위치를 드래그하던 원래 자리로 옮긴다 (스왑)
            currentBlock.SetParent(draggedParent);
            var rt1 = currentBlock.GetComponent<RectTransform>();
            rt1.anchorMin = new Vector2(0.5f, 0.5f);
            rt1.anchorMax = new Vector2(0.5f, 0.5f);
            rt1.pivot = new Vector2(0.5f, 0.5f);
            rt1.anchoredPosition = Vector2.zero;
        }

        // 드래그한 블록을 현재 슬롯으로 이동
        dragged.transform.SetParent(transform);
        var rt2 = dragged.GetComponent<RectTransform>();
        rt2.anchorMin = new Vector2(0.5f, 0.5f);
        rt2.anchorMax = new Vector2(0.5f, 0.5f);
        rt2.pivot = new Vector2(0.5f, 0.5f);
        rt2.anchoredPosition = Vector2.zero;
    }
}
