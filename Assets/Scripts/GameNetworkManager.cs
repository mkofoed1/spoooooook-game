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
        public Vector3 spawn;
       
    }
    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<CreatePlayer>(OnCreatePlayer);
        //foreach(string conn in )
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log(NetworkServer.connections.Count);
        switch (NetworkServer.connections.Count)
        {
            case 0:
                
                CreatePlayer playermsg3 = new CreatePlayer
                {
                    
                    name = "Joe dad",
                    score = 0,
                    spawn = new Vector3(-11,0,-6),
                };
                NetworkClient.Send(playermsg3);
            break;
            
            case 1:
                CreatePlayer playermsg = new CreatePlayer
                {
                    name = "Joe Mama",
                    score = 0,
                    spawn = new Vector3(-9,0,-6)
                };
                NetworkClient.Send(playermsg);
            break;
            
            case 2:
                CreatePlayer playermsg1 = new CreatePlayer
                {
                    name = "karl",
                    score = 0,
                    spawn = new Vector3(-9,0,-6)
                };
                NetworkClient.Send(playermsg1);
            break;
            

            default:
                CreatePlayer playermsg2 = new CreatePlayer
                {
                    name = "julemand",
                    score = 0,
                    spawn = new Vector3(-9,0,-6)
                };
                NetworkClient.Send(playermsg2);
            break;
        }
        

        
    }

    void OnCreatePlayer(NetworkConnectionToClient conn, CreatePlayer msg)
    {
        GameObject playerObj = Instantiate(playerPrefab, msg.spawn,Quaternion.identity);
        playerObj.GetComponent<PlayerMovement>().playername = msg.name;
        playerObj.GetComponent<PlayerMovement>().score = msg.score;
        StartCoroutine(RequestHandler.Instance.CreatePlayer(msg.name, msg.score));
        NetworkServer.AddPlayerForConnection(conn, playerObj);
    }


}
