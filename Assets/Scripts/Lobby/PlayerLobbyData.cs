using System;
using Unity.Collections;
using Unity.Netcode;

[Serializable]
public class PlayerData
{
    public ulong playerId;
    public string playerName;
    public bool isReady;

    public void SetData(PlayerDataTransport playerData)
    {
        playerId = playerData.playerId;
        playerName = playerData.playerName.ToString();
        isReady = playerData.isReady;
    }

}
public struct PlayerDataTransport : INetworkSerializable
{
    public ulong playerId;
    public FixedString64Bytes playerName;
    public bool isReady;

    public PlayerDataTransport(PlayerData playerData)
    {
        playerId = playerData.playerId;
        playerName = playerData.playerName;
        isReady = playerData.isReady;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerId);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref isReady);
    }
}
