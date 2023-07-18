using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject playMenu;

    public void GithubLink()
    {
        Application.OpenURL("https://github.com/UfukAntep/OnlineTradingCardGame");
    }

    public void ArMode()
    {
        Application.LoadLevel(2);
    }

    public void GameMode()
    {
        Application.LoadLevel(1);
    }

    public void PlayMenuClose()
    {
        playMenu.SetActive(false);
    }
    public void PlayMenu()
    {
        playMenu.SetActive(true);
    }
}
