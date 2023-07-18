using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class CardGameManager : SingletonDestroy<CardGameManager>
{
    // Oyuncu ve rakip kartlarını temsil eden listeler
    public List<Card> playerHand;
    public GameObject playerCards;
    public List<Card> opponentHand;

    // Kartların yerleştirilebileceği slotları temsil eden dizi
    public GameObject[] playerSlots;
    public GameObject[] enemySlots;

    public List<GameObject> playerMonsters;
    public List<GameObject> enemyMonsters;

    private bool gamefinished;
    // Oyun durumu
    public bool playerTurn = true;

    private void Start()
    {
        for (int i = 0; i < playerCards.transform.childCount; i++)
        {
            playerCards.transform.GetChild(i).GetComponent<DragAndDrop>().dragCard = playerHand[i];
        }
    }

    private void Update()
    {
        if (enemyMonsters.Count <= 0 && opponentHand.Count <= 0)
        {
            GameManager.Instance.turnText.gameObject.SetActive(true);
            GameManager.Instance.turnText.text = "PLAYER WIN";
            gamefinished = true;
        } else if (playerHand.Count <= 0 && playerMonsters.Count > 0 && playerTurn && !gamefinished)
        {
            CheckMonsters();
            
            PlayerMonstersCanAttack(true);
            StartCoroutine(MonstersCanAttack(playerMonsters, 2f));
            GameManager.Instance.SwitchTurnWithDelay(3f);
            playerTurn = false;
        } 

        if (playerMonsters.Count <= 0 && playerHand.Count <= 0)
        {
            GameManager.Instance.turnText.gameObject.SetActive(true);
            GameManager.Instance.turnText.text = "ENEMY WIN";
            gamefinished = true;
        } else if (opponentHand.Count <= 0 && enemyMonsters.Count > 0 && !playerTurn 
                   && !(playerMonsters.Count <= 0 && playerHand.Count <= 0) && !gamefinished)
        {
            CheckMonsters();
            for (int i = 0; i < enemyMonsters.Count; i++)
            {
                enemyMonsters[i].GetComponent<AIMove>().isAttacked = false;
            }
            StartCoroutine(MonstersCanAttack(enemyMonsters, 2f));
            playerTurn = true;
        }
    }

    public void CheckMonsters()
    {
        for (int i = 0; i < playerSlots.Length; i++)
        {
            if (playerSlots[i].transform.childCount > 0)
            {
                if (!playerMonsters.Contains(playerSlots[i].transform.GetChild(0).gameObject))
                    playerMonsters.Add(playerSlots[i].transform.GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < enemySlots.Length; i++)
        {
            if (enemySlots[i].transform.childCount > 0)
            {
                if (!enemyMonsters.Contains(enemySlots[i].transform.GetChild(0).gameObject))
                    enemyMonsters.Add(enemySlots[i].transform.GetChild(0).gameObject);
            }
        }
    }

    public IEnumerator OpponentTurn()
    {
        yield return new WaitForSeconds(3f);
        
        Card cardToPlay = opponentHand[Random.Range(0, opponentHand.Count)];
        int emptySlotIndex = GetEmptySlotIndex(enemySlots);
        
        if (emptySlotIndex != -1)
        {
            enemySlots[emptySlotIndex].GetComponent<CardSlot>().PlaceCard(cardToPlay);
            CheckMonsters();
            StartCoroutine(MonstersCanAttack(enemyMonsters, 2f));
            opponentHand.Remove(cardToPlay);
        }
        
        playerTurn = true;
        PlayerMonstersCanAttack(true);
        StartCoroutine(MonstersCanAttack(playerMonsters, 2f));
        GameManager.Instance.SwitchTurn();
    }

    private void PlayerMonstersCanAttack(bool state)
    {
        for (int i = 0; i < playerMonsters.Count; i++)
        {
            playerMonsters[i].GetComponent<AIMove>().isAttacked = !state;
        }
    }

    int GetEmptySlotIndex(GameObject[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<CardSlot>().isEmpty)
            {
                return i;
            }
        }
        return -1;
    }

    IEnumerator MonstersCanAttack(List<GameObject> monsters, float waitTime)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            yield return new WaitForSeconds(waitTime);
            monsters[i].GetComponent<AIMove>().canAttack = true;
        }
    }
}
