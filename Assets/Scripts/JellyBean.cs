using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class JellyBean : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {   
        if (other.transform.tag == "player")
        {
            
            Destroy(gameObject);
        }
    }
}
