using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    public List<Card> deckOfCards = new List<Card>();

    public void Shuffle()
    {
        for(int i = 0; i<deckOfCards.Count; i++)
        {
            Card temp = deckOfCards[i];
            int randIndex = UnityEngine.Random.Range(i, deckOfCards.Count);
            deckOfCards[i] = deckOfCards[randIndex];
            deckOfCards[randIndex] = temp;
        }
    }

    public void CreateDeck()
    {
        CreateSpades();

        CreateClubs();

        CreateHearts();

        CreateDiamonds();
    }

    private void CreateDiamonds()
    {
        for(int i = 0; i < 13; i++)
        {
            Card newCard = new Card();

            newCard.cardSuit = 3;

            newCard.faceValue = i;

            deckOfCards.Add(newCard);
        }
    }

    private void CreateHearts()
    {
        for (int i = 0; i < 13; i++)
        {
            Card newCard = new Card();

            newCard.cardSuit = 2;

            newCard.faceValue = i;

            deckOfCards.Add(newCard);
        }
    }

    private void CreateClubs()
    {
        for (int i = 0; i < 13; i++)
        {
            Card newCard = new Card();

            newCard.cardSuit = 1;

            newCard.faceValue = i;

            deckOfCards.Add(newCard);
        }
    }

    private void CreateSpades()
    {
        for (int i = 0; i < 13; i++)
        {
            Card newCard = new Card();

            newCard.cardSuit = 0;

            newCard.faceValue = i;

            deckOfCards.Add(newCard);
        }
    }
}
