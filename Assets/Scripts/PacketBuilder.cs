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

    public static Buffer Play(int suit, int val)
    {
        Buffer packet = Buffer.Alloc(6);

        packet.WriteString("PLAY");
        packet.WriteUInt8((byte)suit, 4);
        packet.WriteUInt8((byte)val, 5);

        return packet;
    }

}
