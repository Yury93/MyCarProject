using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TouchButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector]
    public bool Pressed;
    Image touchImage;
    float color = 1f;
    Color upColor, downColor;

    void Start()
    {
        touchImage = GetComponent<Image>();
        upColor = Color.red;
        downColor = Color.black;
        touchImage.color = upColor;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        touchImage.color = downColor;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
        touchImage.color = upColor;
    }
}