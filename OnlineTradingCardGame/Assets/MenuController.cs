using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject playMenu,teamMenu;

    public void GithubLink()
    {
        Application.OpenURL("https://github.com/UfukAntep/OnlineTradingCardGame");
    }

    public void MainMenu()
    {
        Application.LoadLevel(0);
    }

    public void ArMode()
    {
        Application.LoadLevel(2);
    }

    public void GameMode()
    {
        Application.LoadLevel(1);
    }

    public void PlayMenu()
    {
        playMenu.SetActive(!playMenu.activeSelf);
    }
    public void TeamMenu()
    {
        teamMenu.SetActive(!teamMenu.activeSelf);
    }
}
