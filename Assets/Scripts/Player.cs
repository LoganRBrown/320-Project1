using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*
     * TO DO:
     * Create a variable to handle a player's points
     * create a List to handle a player's hand of cards
     * create a List to handle a player's pot of cards for a round
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

    public int playerScore;

    public Card cardPrefab;

    public List<Card> playerHand = new List<Card>();

    public List<Card> playerPot = new List<Card>();


    void Start()
    {
        
    }

    
    void Update()
    {
        
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

    /// <summary>
    /// This function handles when a player chooses a card to play for their turn in a round.
    /// </summary>
    public void PlayerChoseCard()
    {

    }

    public void DisplayPlayerHand()
    {

        for(int i = 0; i < playerHand.Count; i++)
        {
            Card card = Instantiate(cardPrefab);

            card.Init(() => { ButtonClicked(card);  });
        }

    }

    void ButtonClicked(Card card)
    {
        //put the packet send code here.
    }

    public void EndOfRound()
    {

    }
}
