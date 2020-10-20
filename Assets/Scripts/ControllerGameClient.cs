using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using TMPro;
using System;
using System.Text.RegularExpressions;

public class ControllerGameClient : MonoBehaviour
{
    static public ControllerGameClient singleton;
    
    TcpClient socket = new TcpClient();

    Buffer buffer = Buffer.Alloc(0);

    public TMP_InputField inputHost;
    public TMP_InputField inputPort;
    public TMP_InputField inputUsername;

    public Transform panelHostDetails;
    public Transform panelUsername;
    public HeartsGame panelGameplay;

    public TextMeshProUGUI chatWindow;

    public TMP_InputField inputChat;

    void Start()
    {
        if (singleton)
        {
            //already set...
            Destroy(gameObject); // there's already on out there.... and we dont want two
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject); //dont destroy when loading new scenes!

            // show connection screen
            panelHostDetails.gameObject.SetActive(true);
            panelUsername.gameObject.SetActive(false);
            panelGameplay.gameObject.SetActive(false);
        }
    }

    public void OnButtonConnect()
    {
        string host = inputHost.text;

        UInt16.TryParse(inputPort.text, out ushort port);

        TryToConnect(host, port);
    }

    public void OnButtonUsername()
    {
        string user = inputUsername.text;

        Buffer packet = PacketBuilder.Join(user);

        SendPacketToServer(packet);
    }

    public void OnButtonStart()
    {
        Buffer packet = PacketBuilder.Start();

        this.gameObject.SetActive(false);

        SendPacketToServer(packet);
    }

    async public void TryToConnect(string host, int port)
    {
        if (socket.Connected) return;

        try
        {
            await socket.ConnectAsync(host, port);

            StartReceivingPackets();
        }
        catch
        {
            print("FAILED TO CONNECT...");
        }
    }

    async private void StartReceivingPackets()
    {

        int maxPacketSize = 4096;
        
        while (socket.Connected)
        {
            byte[] data = new byte[4096];

            try
            {
                int bytesRead = await socket.GetStream().ReadAsync(data, 0, maxPacketSize);

                buffer.Concat(data, bytesRead);

                ProcessPackets();
            }
            catch (Exception e) { }
        }
    }

    void ProcessPackets()
    {
        if (buffer.Length < 4) return; //not enough data in buffer

        string packetIdentifier = buffer.ReadString(0, 4);

        switch (packetIdentifier)
        {
            case "JOIN":
                if (buffer.Length < 5) return;
                byte joinResponse = buffer.ReadUInt8(4);

                // TODO: change which screen we're looking at
                if (joinResponse == 1 || joinResponse == 2 || joinResponse == 3 || joinResponse == 4 || joinResponse == 5 || joinResponse == 6 || joinResponse == 7 || joinResponse == 8)
                {
                    panelGameplay.tableSeat = joinResponse;
                    panelHostDetails.gameObject.SetActive(false);
                    panelUsername.gameObject.SetActive(false);
                    panelGameplay.gameObject.SetActive(true);
                }
                else if (joinResponse == 9)
                {
                    panelHostDetails.gameObject.SetActive(true);
                    panelUsername.gameObject.SetActive(false);
                    panelGameplay.gameObject.SetActive(false);
                }
                else
                {
                    panelHostDetails.gameObject.SetActive(false);
                    panelUsername.gameObject.SetActive(true);
                    panelGameplay.gameObject.SetActive(false);
                    inputUsername.text = ""; 
                }

                buffer.Consume(5);

                break;
            case "STRT":

                if (buffer.Length < 5) return;

                byte playerCount = buffer.ReadUInt8(4);
                byte tableSeat = buffer.ReadUInt8(5);

                panelGameplay.playerCount = playerCount;
                panelGameplay.tableSeat = tableSeat;

                panelGameplay.whatGameState = 1;

                break;
            case "UPDT":
                if (buffer.Length < 15) return; // not enough data for a UPDT packet

                byte gameState = buffer.ReadUInt8(4);
                byte whoseTurn = buffer.ReadUInt8(5);
                byte whoHasLost = buffer.ReadUInt8(6);
                byte potSize = buffer.ReadUInt8(7);

                List<Card> tempList = new List<Card>();

                for (int i = 0; i <= potSize; i++)
                {
                    
                    Card tempCard = new Card();

                    tempCard.ConvertCardValue(buffer.ReadUInt8(i));
                    tempList.Add(tempCard);
                }

                // TODO: Switch to gameplay screen
                panelHostDetails.gameObject.SetActive(false);
                panelUsername.gameObject.SetActive(false);
                panelGameplay.gameObject.SetActive(true);

                panelGameplay.UpdateFromServer(gameState, whoseTurn, whoHasLost, tempList);

                buffer.Consume(15);

                break;
            case "CHAT":

                byte usernameLength = buffer.ReadByte(4);

                ushort messageLength = buffer.ReadUInt8(5);

                int fullPacketLength = 7 + usernameLength + messageLength;

                if (buffer.Length < fullPacketLength) return;

                string username = buffer.ReadString(7, usernameLength);

                string message = buffer.ReadString(7 + usernameLength, messageLength);

                // TODO: Switch to gameplay screen...
                panelHostDetails.gameObject.SetActive(false);
                panelUsername.gameObject.SetActive(false);
                panelGameplay.gameObject.SetActive(true);
                // TODO: Update Chat View

                AddMessageToChatDisplay($"{username}: {message}");

                buffer.Consume(fullPacketLength);

                break;
            case "PASS":

                if (panelGameplay.tableSeat == buffer.ReadUInt8(4))
                { 
                    List<Card> tempListTwo = new List<Card>();

                    Card card1 = new Card();
                    Card card2 = new Card();
                    Card card3 = new Card();

                    card1.ConvertCardValue(buffer.ReadUInt8(5));
                    card2.ConvertCardValue(buffer.ReadUInt8(6));
                    card3.ConvertCardValue(buffer.ReadUInt8(7));

                    tempListTwo.Add(card1);
                    tempListTwo.Add(card2);
                    tempListTwo.Add(card3);

                    panelGameplay.myPlayer.PlayerHasBeenPassedCards(tempListTwo);

                    if (panelGameplay.tableSeat == 1) panelGameplay.listOfPlayers[buffer.ReadUInt8(4)].hasPlayerPassed = true;
                }
                break;
            case "HAND":
                if(panelGameplay.tableSeat == buffer.ReadUInt8(4))
                {

                    for(int i = 0; i <= buffer.ReadUInt8(5); i++)
                    {
                        Card tempCard = new Card();

                        tempCard = tempCard.ConvertCardValue(buffer.ReadUInt8(i+6));

                        panelGameplay.myPlayer.playerHand.Add(tempCard);
                    }

                }
                break;
            case "TPOT":
                if(panelGameplay.tableSeat == buffer.ReadUInt8(4))
                {
                    for (int i = 0; i <= panelGameplay.playerCount; i++)
                    {
                        Card tempCard = new Card();

                        tempCard = tempCard.ConvertCardValue(buffer.ReadUInt8(i+5));

                        panelGameplay.myPlayer.playerPot.Add(tempCard);
                    }
                }
                break;
            case "RSCR":
                panelGameplay.listOfPlayers.ForEach(p =>
                {
                    int offset = 4;

                    p.playerScore = buffer.ReadUInt8(offset++);

                });
                break;
            default:
                print("unknown packet Identifier...");

                buffer.Clear();
                break;
        }
    }

    public void AddMessageToChatDisplay(string txt)
    {
        chatWindow.text += $"{txt}\n";
    }

    public void UserDoneEditingMessage(string txt)
    {
        if(!new Regex(@"^(\s|\t)*$").IsMatch(txt))
        {
            SendPacketToServer(PacketBuilder.Chat(txt));
            inputChat.text = "";
        }

        inputChat.Select();
        inputChat.ActivateInputField();
    }

    async public void SendPacketToServer(Buffer packet)
    {
        if (!socket.Connected) return; // not connected to server...

        await socket.GetStream().WriteAsync(packet.bytes, 0, packet.bytes.Length);
    }

    public void SendPlayPacket(int x, int y)
    {
        SendPacketToServer( PacketBuilder.Play(x, y) );
        
    }

    public void SendPassPacket(Card a, Card b, Card c)
    {
        SendPacketToServer(PacketBuilder.Pass(a.cardSuit, a.faceValue, b.cardSuit, b.faceValue, c.cardSuit, c.faceValue));
    }
}
