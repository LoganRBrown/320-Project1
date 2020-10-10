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

    public void Init(HandPos pos, UnityAction callBack)
    {
        this.pos = pos;

        Button bttn = GetComponent<Button>();

        bttn.onClick.AddListener(callBack);
    }
}
