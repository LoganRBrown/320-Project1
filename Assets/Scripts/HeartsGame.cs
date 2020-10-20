using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public Player myPlayer;

    public Transform panelTableTop;

    public List<Player> listOfPlayers = new List<Player>();

    public List<Card> tablePot = new List<Card>();

    private Card[,] tableUI;

    public int playerCount = 0;

    public int cardPassCounter = 0;

    public int tableSeat = 0;

    public int whatGameState = 0;
    /*
     *  This variable is used for communicating what game state we're in with the server.
     *  0: Pre game
     *  1: The Cards are currently being dealt
     *  2: players are currently selecting cards to pass
     *  3: the game is being played
     *  4: the round is over.
     */

    private bool doOnce = true; // used for the handle game state function.

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void HowManyPlayers(int playerCount)
    {

        for(int i = 0; i <= playerCount; i++)
        {
            Player tempPlayer = new Player();

            listOfPlayers.Add(tempPlayer);
        }

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
        bool someoneShotMoon = false;

        listOfPlayers.ForEach(x => { x.EndOfRound(); });

        listOfPlayers.ForEach(p => { 
            if (p.hasShotMoon) 
            { 
                someoneShotMoon = true;
            }
            if (!p.hasShotMoon && someoneShotMoon) p.playerScore += 26;
        });
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

        myPlayer.DisplayPlayerHand();

    }

    public void HandlePot(List<Card> pot)
    {

        int columns = playerCount / 2;
        int rows = playerCount / 2;

        tableUI = new Card[columns, rows];

        for(int x = 0; x < columns; x++)
        {
            for(int y = 0; y < rows; y++)
            {
                for(int i = 0; i <= pot.Count; i++)
                {
                    pot[i] = Instantiate(myPlayer.cardPrefab, panelTableTop);

                    pot[i].Init(new HandPos(x, y), null, this);

                    tableUI[x, y] = pot[i];
                }
            }
        }

    }

    public void UpdateFromServer(byte gameState, byte whoseTurn, byte whoHasLost, List<Card> cards)
    {
        // TODO: update all of the interface to reflect game state:
        // - Whose Turn
        // - 9 space on board
        // - status

        HandleGameState();

        whatGameState = gameState;

        if (tableSeat == whoseTurn) myPlayer.isTurn = true;

        HandlePot(cards);

        

    }

    public void HandleGameState()
    {
        /*
         * TODO
         * 
         * This fucntion needs to handle the hearts game transition from it's different states.
         * Those states are:
         * Dealiing cards to players
         * Players passing cards to other players
         * Playing the current Round
         * Ending the Round
         */

        if (doOnce)
        {
            CreateDecks(playerCount);

            DealCards();

            doOnce = false;
        }

        switch (whatGameState)
        {
            /*
             *  This variable is used for communicating what game state we're in with the server.
             *  0: Pre game
             *  1: The Cards are currently being dealt
             *  2: players are currently selecting cards to pass
             *  3: the game is being played
             *  4: the round is over.
             */
            case 0:
                if (tableSeat == 1) HowManyPlayers(playerCount);
                break;
            case 1:
                if (tableSeat == 1)
                {
                    CreateDecks(playerCount);
                    DealCards();
                    listOfPlayers.ForEach(p => {

                        ControllerGameClient.singleton.SendPacketToServer(PacketBuilder.Hand(p, listOfPlayers.IndexOf(p)));
                    
                    });
                }
                whatGameState = 2;
                break;
            case 2:
                if (listOfPlayers.TrueForAll(x => x.hasPlayerPassed == true))
                {
                    //Have the server check if all players have passed their cards and are ready to proceed to play.
                    whatGameState = 3;
                }

                break;
            case 3:

                

                break;
            case 4:

                ControllerGameClient.singleton.SendPacketToServer(PacketBuilder.PPot(myPlayer, tableSeat));

                if (tableSeat == 1) ControllerGameClient.singleton.SendPacketToServer(PacketBuilder.Rscr());

                break;
        }


    }
}
