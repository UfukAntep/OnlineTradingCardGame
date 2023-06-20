using UnityEngine;
using UnityEngine.UI;

public class FitBackground : MonoBehaviour
{
    public Canvas mainCanvas;
    public Sprite sprite;
    private Vector2 _sizeDelta;
       
    void Start()
    {
        float width = sprite.bounds.size.x;
        float height = sprite.bounds.size.y;
        _sizeDelta = mainCanvas.GetComponent<RectTransform>().sizeDelta;
        float flagButtonHeight = height * _sizeDelta.x / width;
        float flagButtonWidth = _sizeDelta.x;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(flagButtonWidth, flagButtonHeight);
        Image imageComponent = gameObject.AddComponent<Image>();
        imageComponent.sprite = sprite;
    }
}
