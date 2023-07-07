using UnityEngine;

public class ARCardSlot : MonoBehaviour
{
    public TeamColor Team;
    public bool isEmpty = true;
    
    public Card _card;
    // public Image cardImage;
    public GameObject _monster;

    private void Start()
    {
        PlayerPrefs.SetInt("spawnMonster",0);
    }

    private void Update()
    {
        print(PlayerPrefs.GetInt("spawnMonster"));

        if (PlayerPrefs.GetInt("spawnMonster") != 0)
        {
            PlaceCard();
        }
    }

    public void PlaceCard()
    {

            // cardImage.sprite = card.backSide; //oynayisa gore on veya arka yuz gorunecek

            _monster = Instantiate(_card.monsterObject, transform);
            isEmpty = false;

            if (Team == TeamColor.Blue)
            {
                CardGameManager.Instance.playerTurn = false;
                CardGameManager.Instance.playerHand.Remove(_card);

                for (int i = 0; i < CardGameManager.Instance.enemyMonsters.Count; i++)
                {
                    CardGameManager.Instance.enemyMonsters[i].GetComponent<AIMove>().isAttacked = false;
                }
                CardGameManager.Instance.StartCoroutine("OpponentTurn");
            }

        }
        
    
    
}