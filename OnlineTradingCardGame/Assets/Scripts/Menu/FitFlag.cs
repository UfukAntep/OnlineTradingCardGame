using UnityEngine;
using UnityEngine.UI;

public class FitFlag : MonoBehaviour
{
    public Canvas mainCanvas;
    public Sprite sprite;
    private Vector2 _sizeDelta;
       
   void Start()
   {
       float width = sprite.bounds.size.x;
       float height = sprite.bounds.size.y;
       _sizeDelta = mainCanvas.GetComponent<RectTransform>().sizeDelta;
       float flagButtonHeight = height * _sizeDelta.x / 5 / width;
       float flagButtonWidth = _sizeDelta.x / 5;
       gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(flagButtonWidth, flagButtonHeight);
       float x = gameObject.GetComponent<RectTransform>().position.x;
       float y = gameObject.GetComponent<RectTransform>().position.y;
       float z = gameObject.GetComponent<RectTransform>().position.z;
       gameObject.GetComponent<RectTransform>().position = new Vector3(x,y+12,z);
       Image imageComponent = gameObject.AddComponent<Image>();
       imageComponent.sprite = sprite;
   }
}
