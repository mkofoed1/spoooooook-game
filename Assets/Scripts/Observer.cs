using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Observer : NetworkBehaviour
{
    public GameEnding gameEnding;
    private GameObject spiller;

    bool m_IsPlayerInRange;
    
    [Client]
    void OnTriggerEnter (Collider other)
    {        
        if (other.tag == "player")
        {
            spiller = other.gameObject;
            
            CheckVision(other.transform);
            CmdPlayerCaught(other.gameObject.GetComponent<NetworkIdentity>());
            StartCoroutine(RequestHandler.Instance.DeletePlayer(spiller.GetComponent<PlayerMovement>().playername));
        }
    }

    void OnTriggerExit (Collider other)
    {
    }

    void CheckVision (Transform player)
    {
        Vector3 direction = player.position - transform.position + Vector3.up;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit raycastHit;
        
        if (Physics.Raycast (ray, out raycastHit))
        {
            
            if (raycastHit.collider.transform == player)
            {      
                
            }
        }
    }

    [Command(requiresAuthority = false)]
    void CmdPlayerCaught(NetworkIdentity networkIdentity)
    {   
        gameEnding.CaughtPlayer (networkIdentity);
    }

}
