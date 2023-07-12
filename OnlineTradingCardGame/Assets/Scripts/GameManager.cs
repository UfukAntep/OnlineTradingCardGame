using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : SingletonDestroy<GameManager>
{
    [SerializeField] public bool isGameStart = false;
    [SerializeField] public bool isGameStop = false;

    public TeamColor PlayableColor;
    public TeamColor PlayerColor;
    
    public TextMeshProUGUI turnText;

    private void Start()
    {
        turnText.text = "Your Turn";
    }

    public void BeginGame()
    {
        GameUtils.GameResume();
        isGameStart = true;
        isGameStop = false;
    }
    
    public void PauseGame()
    {
        isGameStart = false;
        isGameStop = true;
        GameUtils.GameStop();
    }

    public void SwitchTurn()
    {
        switch (PlayableColor)
        {
            case TeamColor.Blue:
                PlayableColor = TeamColor.Red;
                StartCoroutine(TurnTextState(true, 0f));
                turnText.text = "Opponent Turn";
                StartCoroutine(TurnTextState(false, 3f));
                break;
            case TeamColor.Red:
                PlayableColor = TeamColor.Blue;
                StartCoroutine(TurnTextState(true, 0f));
                turnText.text = "Your Turn";
                StartCoroutine(TurnTextState(false, 3f));
                break;
        }
    }

    private IEnumerator TurnTextState(bool state, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        turnText.gameObject.SetActive(state);
    }
}
