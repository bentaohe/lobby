using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity.Netcode;

public class NetworkHandler : NetworkBehaviour
{
    void Start()
    {
        NetworkManager.Singleton.OnClientStarted += OnClientStarted;
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

 
    private void PrintMe()
    {
        if (IsServer)
        {
            NetworkHelper.Log($"I AM a Server!{NetworkManager.ServerClientId}");
        }
        if (IsHost)
        {
            NetworkHelper.Log($"I AM a Host! {NetworkManager.ServerClientId}/{NetworkManager.LocalClientId}");
        }
        if (IsClient)
        {
            NetworkHelper.Log($"I AM a Client! {NetworkManager.LocalClientId}");
        }
        if (!IsServer && !IsClient)
        {
            NetworkHelper.Log("I AM Nothing Yet!");
            
        }
    }

    private void OnClientStarted()
    {
        NetworkHelper.Log("!! Client Started !!");
        NetworkManager.OnClientConnectedCallback += ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback += ClientOnClientDisconnected;
        NetworkManager.OnClientStopped += ClientOnClientStopped;
        PrintMe();
    }

    private void OnServerStarted()
    {
        NetworkHelper.Log("!! Server Started !!");
        NetworkManager.OnClientConnectedCallback += ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback += ServerOnClientDisconnected;
        NetworkManager.OnServerStopped += ServerOnServerStopped;
        PrintMe();
    }

    private void ServerOnServerStopped(bool indicator)
    {
        NetworkHelper.Log("!! Server Stopped !!");
        NetworkManager.OnClientConnectedCallback -= ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ServerOnClientDisconnected;
        NetworkManager.OnServerStopped -= ServerOnServerStopped;
        PrintMe();
    }

    private void ServerSetup()
    {

    }

    private void ServerOnClientConnected(ulong clientId)
    {
        // This method is called when a client connects to the server.
        NetworkHelper.Log($"Client {clientId} connected to the server.");
    }

    private void ServerOnClientDisconnected(ulong clientId)
    {
        // This method is called when a client disconnects from the server.
        NetworkHelper.Log($"Client {clientId} disconnected from the server.");
    }

    private void ClientOnClientConnected(ulong clientId)
    {
        NetworkHelper.Log($"I have connected {clientId}");
    }
    private void ClientOnClientDisconnected(ulong clientId)
    {
        NetworkHelper.Log($"I have disconnected {clientId}");
    }
    private void ClientOnClientStopped(bool indicator)
    {
        NetworkHelper.Log("!! Client Stopped !!");
        NetworkManager.OnClientConnectedCallback -= ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ClientOnClientDisconnected;
        NetworkManager.OnClientStopped -= ClientOnClientStopped;
        PrintMe();
    }
}