using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public struct HandPos
{
    public int x;
    public int y;

    public HandPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Card : MonoBehaviour
{
    [HideInInspector] public int cardSuit;
    /* Suit Values:
     * 0: Spades
     * 1: Clubs
     * 2: Hearts
     * 3: Diamonds
     */

    [HideInInspector] public int faceValue;
    /* Face Values:
     * 0: Ace
     * 1: Two
     * 2: Three
     * 3: Four
     * 4: Five
     * 5: Six
     * 6: Seven
     * 7: Eight
     * 8: Nine
     * 9: Ten
     * 10: Jack
     * 11: Queen
     * 12: King
     */

    [HideInInspector] public int pointValue = 0;
    /*
     * This can be one of three values and is used to determine a cards point value
     * 0: The card is not a heart and is worth no points
     * 1: The card is a heart and is worth 1 point
     * 13: This card is the queen of spades and is worth 13 points
     * 
     * The total value of points in a deck should never exceed 26.
     */

    [HideInInspector] public Player ownedBy;

    [HideInInspector] public HeartsGame belongsToGame;

    public TextMeshProUGUI suitText; //Text Mesh Pro For displaying the card suit

    public TextMeshProUGUI valueText; //Text Mesh Pro for displaying the Value of the card

    public HandPos pos;

    /*void Start()
    {
        AssignSuit();
        AssignValue();
    }*/

    /// <summary>
    /// This function takes the value of cardSuit integer and uses the value to output that suit into a Text Mesh Pro text box.
    /// </summary>
    public void AssignSuit()
    {
        switch (cardSuit)
        {
            case 0:
                suitText.text = "Spades";
                break;
            case 1:
                suitText.text = "Clubs";
                break;
            case 2:
                suitText.text = "Hearts";
                pointValue = 1;
                break;
            case 3:
                suitText.text = "Diamonds";
                break;
        }
    }

    /// <summary>
    /// This function takes the value of the faceValue integer and uses the value to output the card value to a Text Mesh Pro text box.
    /// </summary>
    public void AssignValue()
    {
        switch (faceValue)
        {
            case 0:
                valueText.text = "Ace";
                break;
            case 1:
                valueText.text = "Two";
                break;
            case 2:
                valueText.text = "Three";
                break;
            case 3:
                valueText.text = "Four";
                break;
            case 4:
                valueText.text = "Five";
                break;
            case 5:
                valueText.text = "Six";
                break;
            case 6:
                valueText.text = "Seven";
                break;
            case 7:
                valueText.text = "Eight";
                break;
            case 8:
                valueText.text = "Nine";
                break;
            case 9:
                valueText.text = "Ten";
                break;
            case 10:
                valueText.text = "Jack";
                break;
            case 11:
                valueText.text = "Queen";
                if (cardSuit == 0) pointValue = 13;
                break;
            case 12:
                valueText.text = "King";
                break;
        }
    }

    public void Init(HandPos pos, UnityAction callBack, Player player)
    {
        this.pos = pos;

        Button bttn = GetComponent<Button>();

        bttn.onClick.AddListener(callBack);

        ownedBy = player;
    }

    public void Init(HandPos pos, UnityAction callBack, HeartsGame game)
    {
        this.pos = pos;

        Button bttn = GetComponent<Button>();

        bttn.onClick.AddListener(callBack);

        belongsToGame = game;
    }

    public Card ConvertCardValue(byte cv)
    {

        switch (cv)
        {
            case 1:
                cardSuit = 0;
                faceValue = 0;
                break;
            case 2:
                cardSuit = 0;
                faceValue = 1;
                break;
            case 3:
                cardSuit = 0;
                faceValue = 2;
                break;
            case 4:
                cardSuit = 0;
                faceValue = 3;
                break;
            case 5:
                cardSuit = 0;
                faceValue = 4;
                break;
            case 6:
                cardSuit = 0;
                faceValue = 5;
                break;
            case 7:
                cardSuit = 0;
                faceValue = 6;
                break;
            case 8:
                cardSuit = 0;
                faceValue = 7;
                break;
            case 9:
                cardSuit = 0;
                faceValue = 8;
                break;
            case 10:
                cardSuit = 0;
                faceValue = 9;
                break;
            case 11:
                cardSuit = 0;
                faceValue = 10;
                break;
            case 12:
                cardSuit = 0;
                pointValue = 13;
                faceValue = 11;
                break;
            case 13:
                cardSuit = 0;
                faceValue = 12;
                break;
            case 14:
                cardSuit = 1;
                faceValue = 0;
                break;
            case 15:
                cardSuit = 1;
                faceValue = 1;
                break;
            case 16:
                cardSuit = 1;
                faceValue = 2;
                break;
            case 17:
                cardSuit = 1;
                faceValue = 3;
                break;
            case 18:
                cardSuit = 1;
                faceValue = 4;
                break;
            case 19:
                cardSuit = 1;
                faceValue = 5;
                break;
            case 20:
                cardSuit = 1;
                faceValue = 6;
                break;
            case 21:
                cardSuit = 1;
                faceValue = 7;
                break;
            case 22:
                cardSuit = 1;
                faceValue = 8;
                break;
            case 23:
                cardSuit = 1;
                faceValue = 9;
                break;
            case 24:
                cardSuit = 1;
                faceValue = 10;
                break;
            case 25:
                cardSuit = 1;
                faceValue = 11;
                break;
            case 26:
                cardSuit = 1;
                faceValue = 12;
                break;
            case 27:
                cardSuit = 2;
                pointValue = 1;
                faceValue = 0;
                break;
            case 28:
                cardSuit = 2;
                pointValue = 1;
                faceValue = 1;
                break;
            case 29:
                cardSuit = 2;
                pointValue = 1;
                faceValue = 2;
                break;
            case 30:
                cardSuit = 2;
                pointValue = 1;
                faceValue = 3;
                break;
            case 31:
                cardSuit = 2;
                pointValue = 1;
                faceValue = 4;
                break;
            case 32:
                cardSuit = 2;
                pointValue = 1;
                faceValue = 5;
                break;
            case 33:
                cardSuit = 2;
                pointValue = 1;
                faceValue = 6;
                break;
            case 34:
                cardSuit = 2;
                pointValue = 1;
                faceValue = 7;
                break;
            case 35:
                cardSuit = 0;
                pointValue = 1;
                faceValue = 8;
                break;
            case 36:
                cardSuit = 2;
                pointValue = 1;
                faceValue = 9;
                break;
            case 37:
                cardSuit = 2;
                pointValue = 1;
                faceValue = 10;
                break;
            case 38:
                cardSuit = 2;
                pointValue = 1;
                faceValue = 11;
                break;
            case 39:
                cardSuit = 2;
                pointValue = 1;
                faceValue = 12;
                break;
            case 40:
                cardSuit = 3;
                faceValue = 0;
                break;
            case 41:
                cardSuit = 3;
                faceValue = 1;
                break;
            case 42:
                cardSuit = 3;
                faceValue = 2;
                break;
            case 43:
                cardSuit = 3;
                faceValue = 3;
                break;
            case 44:
                cardSuit = 3;
                faceValue = 4;
                break;
            case 45:
                cardSuit = 3;
                faceValue = 5;
                break;
            case 46:
                cardSuit = 3;
                faceValue = 6;
                break;
            case 47:
                cardSuit = 3;
                faceValue = 7;
                break;
            case 48:
                cardSuit = 3;
                faceValue = 8;
                break;
            case 49:
                cardSuit = 3;
                faceValue = 9;
                break;
            case 50:
                cardSuit = 3;
                faceValue = 10;
                break;
            case 51:
                cardSuit = 3;
                faceValue = 11;
                break;
            case 52:
                cardSuit = 3;
                faceValue = 12;
                break;

        }

        return this;
    }

}
