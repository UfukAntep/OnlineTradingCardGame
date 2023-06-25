using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public MenuCard card;
    public Button button;
    public Image icon;
    public TMP_Text text;
    private int _sceneNumber;
    void Start()
    {
        button.name = card.name;
        icon.sprite = card.artwork;
        _sceneNumber = card.sceneNumber;
        button.onClick.AddListener(ChangeScene);
        text.text = card.name.ToUpper();
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(_sceneNumber);
    }
}
