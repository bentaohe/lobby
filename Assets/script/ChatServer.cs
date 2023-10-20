using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChatServer : NetworkBehaviour
{
    public ChatUi chatUi;
    const ulong SYSTEM_ID = ulong.MaxValue;
    private ulong[] dmClientIds = new ulong[2];

    void Start()
    {
        chatUi.printEnteredText = false;
        chatUi.MessageEntered += OnChatUiMessageEntered;

        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback += ServerOnClientConnected;
            if (IsHost)
            {
                DisplayMessageLocally(SYSTEM_ID, $"You are the host and client {NetworkManager.LocalClientId}");
            }
            else
            {
                DisplayMessageLocally(SYSTEM_ID, "You are the server");
            }
        }
        else
        {
            DisplayMessageLocally(SYSTEM_ID, $"You are the client {NetworkManager.LocalClientId}");
        }
    }
    private void ServerOnClientConnected(ulong clientId)
    {
        ServerSendDirectMessage($"I ({NetworkManager.LocalClientId}) see you ({clientId}) have connected to the server, well done", NetworkManager.LocalClientId, clientId);
    }
        

    private void DisplayMessageLocally(ulong from, string message)
    {
        string fromStr = $"Player {from}";
        Color textColor = chatUi.defaultTextColor;

        if (from == NetworkManager.Singleton.LocalClientId)
        {
            fromStr = "you";
            textColor = Color.magenta;
        }
        else if (from == SYSTEM_ID)
        {
            fromStr = "SYS";
            textColor = Color.green;
        }
        chatUi.addEntry(fromStr, message, textColor);
    }

    private void OnChatUiMessageEntered(string message)
    {
        SendChatMessageServerRpc(message);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendChatMessageServerRpc(string message, ServerRpcParams serverParams = default)
    {
        if (message.StartsWith("@"))
        {
            string[] parts = message.Split(" ");
            string clientIdStr = parts[0].Replace("@", "");
            ulong toClientId = ulong.Parse(clientIdStr);
            ServerSendDirectMessage(message, serverParams.Receive.SenderClientId, toClientId);
        }
        else { }
        ReceiveChatMessageClientRpc(message, serverParams.Receive.SenderClientId);
    }

    [ClientRpc]
    public void ReceiveChatMessageClientRpc(string message, ulong from, ClientRpcParams clientParams = default)
    {
        DisplayMessageLocally(from, message);
    }
    private void ServerSendDirectMessage(string message, ulong from, ulong to)
    {
        dmClientIds[0] = from;
        dmClientIds[1] = to;
        //ulong[] clientIds = new ulong[2] {
           // from, to };
        ClientRpcParams rpcParams = default;
        rpcParams.Send.TargetClientIds = dmClientIds;

        //clientIds[0] = from;
        //ReceiveChatMessageClientRpc($"<Whisper> {message}", from, rpcParams);

        //clientIds[0] = to;
        ReceiveChatMessageClientRpc($"<Whisper> {message}", from, rpcParams);


    }
}
