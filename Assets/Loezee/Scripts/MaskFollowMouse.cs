using UnityEngine;

public class MaskFollowMouseWorldSpace : MonoBehaviour
{
    [SerializeField] private Canvas canvas;             
    [SerializeField] private RectTransform clampWithin;  
    [SerializeField] private Vector3 worldOffset;       

    private RectTransform rt;

    void Awake()
    {
        rt = transform as RectTransform;
        if (!canvas) canvas = GetComponentInParent<Canvas>();
        if (!clampWithin) clampWithin = rt.parent as RectTransform;
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        Camera cam = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;

        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                clampWithin, Input.mousePosition, cam, out worldPoint))
        {
            rt.position = worldPoint + worldOffset;
        }
    }
}