using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameNetworkManager : NetworkManager
{
    public struct CreatePlayer : NetworkMessage
    {
        public string name;
        public int score;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<CreatePlayer>(OnCreatePlayer);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        CreatePlayer playermsg = new CreatePlayer
        {
            name = "Joe Mama",
            score = 0,
        };
    }

    void OnCreatePlayer(NetworkConnectionToClient conn, CreatePlayer msg)
    {
        GameObject playerObj = Instantiate(playerPrefab);

        playerObj.GetComponent<PlayerMovement>().name = msg.name;
        playerObj.GetComponent<PlayerMovement>().score = msg.score;

        NetworkServer.AddPlayerForConnection(conn, playerObj);
    }
}
