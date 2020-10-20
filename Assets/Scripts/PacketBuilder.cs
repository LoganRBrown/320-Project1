using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PacketBuilder
{

    public static Buffer Join(string username)
    {
        //Buffer packet = Buffer.From("JOIN");
        //packet.Concat(new byte[] { (byte)username.Length });

        int packetLength = 5 + username.Length;
        Buffer packet = Buffer.Alloc(packetLength);

        packet.WriteString("JOIN");
        packet.WriteUInt8((byte)username.Length, 4);
        packet.WriteString(username, 5);

        return packet;
    }

    public static Buffer Chat(string message)
    {
        int packetLength = 5 + message.Length;
        Buffer packet = Buffer.Alloc(packetLength);

        packet.WriteString("CHAT");
        packet.WriteUInt8((byte)message.Length, 4);
        packet.WriteString(message, 5);

        return packet;
    }

    public static Buffer Start()
    {
        Buffer packet = Buffer.Alloc(4);

        packet.WriteString("STRT");

        return packet;
    }

    public static Buffer Play(int suit, int val)
    {
        Buffer packet = Buffer.Alloc(6);

        packet.WriteString("PLAY");
        packet.WriteUInt8((byte)suit, 4);
        packet.WriteUInt8((byte)val, 5);

        return packet;
    }

    public static Buffer Pass(int suit1, int val1, int suit2, int val2, int suit3, int val3)
    {
        Buffer packet = Buffer.Alloc(10);

        packet.WriteString("PASS");
        packet.WriteUInt8((byte)suit1, 4);
        packet.WriteUInt8((byte)val1, 5);
        packet.WriteUInt8((byte)suit2, 6);
        packet.WriteUInt8((byte)val2, 7);
        packet.WriteUInt8((byte)suit3, 8);
        packet.WriteUInt8((byte)val3, 9);

        return packet;
    }

    public static Buffer Hand(Player player, int playerSeat)
    {
        Buffer packet = Buffer.Alloc(32);

        int offsetSuit = 5;
        int offsetVal = 6;

        packet.WriteString("HAND");
        packet.WriteUInt8((byte)playerSeat, 4);
        player.playerHand.ForEach(p => {

            packet.WriteUInt8((byte)p.cardSuit, offsetSuit += 2);
            packet.WriteUInt8((byte)p.faceValue, offsetVal += 2);

        });

        packet.WriteUInt8((byte)player.playerScore, (offsetSuit + offsetVal));

        return packet;
    }

    public static Buffer PPot(Player player, int playerSeat)
    {
        int potsize = player.playerPot.Count;
        int offsetSuit = 6;
        int offsetVal = 7;

        Buffer packet = Buffer.Alloc(6 + (potsize*2));

        packet.WriteString("PPOT");
        packet.WriteUInt8((byte)playerSeat, 4);
        packet.WriteUInt8((byte)potsize, 5);
        player.playerPot.ForEach(p => {

            packet.WriteUInt8((byte)p.cardSuit, offsetSuit += 2);
            packet.WriteUInt8((byte)p.faceValue, offsetVal += 2);

        });


        return packet;
    }

    public static Buffer Rscr()
    {

        Buffer packet = Buffer.Alloc(4);
        packet.WriteString("RSCR", 0);

        return packet;
    }

}
