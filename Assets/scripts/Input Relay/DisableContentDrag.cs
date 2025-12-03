using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisableContentDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Attach this to the Viewport or Content GameObject of your ScrollRect

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Do nothing (blocks begin drag)
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Do nothing (blocks drag)
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Do nothing (blocks end drag)
    }
}