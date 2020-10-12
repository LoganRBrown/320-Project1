using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using TMPro;
using System;

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
                if (joinResponse == 1 || joinResponse == 2 || joinResponse == 3)
                {
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
            case "UPDT":
                if (buffer.Length < 15) return; // not enough data for a UPDT packet

                byte whoseTurn = buffer.ReadUInt8(4);
                byte gameStatus = buffer.ReadUInt8(5);

                byte[] spaces = new byte[9];
                for(int i = 0; i < 9; i++)
                {
                    spaces[i] = buffer.ReadUInt8(6 + i);
                }

                // TODO: Switch to gameplay screen
                panelHostDetails.gameObject.SetActive(false);
                panelUsername.gameObject.SetActive(false);
                panelGameplay.gameObject.SetActive(true);

                panelGameplay.UpdateFromServer(gameStatus, whoseTurn, spaces);

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

                buffer.Consume(fullPacketLength);

                break;
            default:
                print("unknown packet Identifier...");

                buffer.Clear();
                break;
        }
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
}
