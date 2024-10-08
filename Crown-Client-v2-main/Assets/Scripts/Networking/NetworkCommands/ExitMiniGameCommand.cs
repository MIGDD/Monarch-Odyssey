using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMiniGameCommand : NetworkCommand
{
    public ExitMiniGameCommand(string _gameType, string _miniGameHash, int _score)
    {
        packet = new Packet((int)ClientPackets.exitMiniGame);
        packet.Write(_gameType);
        packet.Write(_miniGameHash);
        packet.Write(_score);
        packetId = (int)ClientPackets.exitMiniGame;
    }
}
