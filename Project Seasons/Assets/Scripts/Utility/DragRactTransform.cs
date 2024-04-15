using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragRactTransform : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    RectTransform _rectTransform;
    Image _image;
    
    [SerializeField]
    Color dragColor = new (255,255,255,255);
    
    [SerializeField]
    Color baseColor = new(255,255,255,255);

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _image.color = dragColor;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _image.color = baseColor;
    }
}
