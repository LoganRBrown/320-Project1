using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*
     * TO DO:
     * Create a variable to handle a player's points -- DONE
     * create a List to handle a player's hand of cards -- DONE
     * create a List to handle a player's pot of cards for a round -- DONE
     * create a function that handles when the player recieves a pot of cards
     * create a function the handles when a player is dealt cards
     * create a function that handles when a player plays a card
     * create a function that handles the end of the round
     * 
     * create the player UI system:
     * UI SYSTEM MUST HAVE:
     * Chat window
     * Hand
     * Player List
     * Score Tracker
     * function to display cards in hand
     */

    private int columns;
    private int rows = 1;

    public int playerScore;

    [HideInInspector] public int playerScoreForRound;

    public Card cardPrefab;

    public bool isTurn = false;

    public bool hasShotMoon = false;

    public bool hasPlayedAllCards = false;

    public List<Card> playerHand = new List<Card>();

    public List<Card> playerPot = new List<Card>();

    public Transform panelPlayerHand;

    private Card[,] handUI;


    void Start()
    {
        
    }

    
    void Update()
    {
        if (playerHand.Count == 0) hasPlayedAllCards = true;
    }

    /// <summary>
    /// This function handles when a player recieves a pot of cards for the round and adds them to the players current pot of cards.
    /// </summary>
    /// <param name="potForRound"> The Pot of cards coming from the end of the round to the player's pot. </param>
    public void PlayerRecievesPot(List<Card> potForRound)
    {
        playerPot.AddRange(potForRound);
    }

    /// <summary>
    /// The function handles when a player is dealt a card to be added to their hand.
    /// </summary>
    /// <param name="card"> The Card to be added to a player's hand. </param>
    public void PlayerDealtCard(Card card)
    {
        playerHand.Add(card);
    }

    public void DisplayPlayerHand()
    {

        columns = playerHand.Count;

        handUI = new Card[columns, rows];

        for(int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Card card = Instantiate(cardPrefab, panelPlayerHand);

                card.Init(new HandPos(x, y), () => { CardClicked(card); }, this);

                handUI[x, y] = card;
            }
        }

    }

    void CardClicked(Card card)
    {
        ControllerGameClient.singleton.SendPlayPacket(card.cardSuit, card.faceValue);
    }

    public void EndOfRound()
    {
        /*
         * TODO: 
         * Add all cards collected for that round to player's point value.
         * determine if player has shot the moon
         * empty out a players pot for that round
         */

        playerPot.ForEach(c => { playerScoreForRound += c.pointValue; }) ;

        if (playerScoreForRound == 26) hasShotMoon = true;

        else playerScore += playerScoreForRound;

        playerPot.Clear();

    }
}
