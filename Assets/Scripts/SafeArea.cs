using UnityEngine;

public class SafeArea : MonoBehaviour
{
    Rect rect;
    RectTransform rectTransform;

    Vector2 minAnchor;
    Vector2 maxAnchor;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rect = Screen.safeArea;

        minAnchor = rect.position;
        maxAnchor = minAnchor + rect.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
    }
}