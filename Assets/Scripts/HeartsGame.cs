using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsGame : MonoBehaviour
{
    /*
     * TO DO:
     * Create a function to handle the rules of the game
     * create a function to handle the players of the game
     * create a function to create decks for the game
     * create a function to handle scoring for a round
     * create a funciton to handle scoring for the game
     * create a function to handle dealing cards to each player
     * create a function to handle the pool of cards
     * Create a state Machine to handle turn order of players
     * 
     */

    public Deck deckForGame;

    public List<Player> listOfPlayers = new List<Player>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void HowManyPlayers(int playerCount)
    {

    }

    public void CreateDecks(int playerCount)
    {
        float cardsToEachPlayer = (52 / playerCount);

        if(cardsToEachPlayer < 8)
        {
            deckForGame.CreateDeck();
            deckForGame.CreateDeck();
        }
        else
        {
            deckForGame.CreateDeck();
        }

        // We shuffle three time because that's the appropriate amount of times to shuffle.
        deckForGame.Shuffle();
        deckForGame.Shuffle();
        deckForGame.Shuffle();
    }

    public void RoundScoring()
    {

    }

    public void GameScoring()
    {

    }

    public void DealCards()
    {

        Card tempCard;

        while (deckForGame.deckOfCards.Count != 0)
        {
            for (int i = 0; i < listOfPlayers.Count; i++)
            {

                tempCard = deckForGame.deckOfCards[0];

                listOfPlayers[i].PlayerDealtCard(tempCard);

                deckForGame.deckOfCards.Remove(tempCard);

            }
        }

    }

    public void HandlePot()
    {

    }

    public void HandleTurnOrder()
    {

    }
}
