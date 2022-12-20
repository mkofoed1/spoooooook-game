using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestHandler : MonoBehaviour
{
    //The local address of the server - URI (Uniform Resource Identifier)
    private string URI = "http://localhost:3000/";

    // Start is called before the first frame update
    private void Start()
    {
        //Starts GetPlayer() as a Coroutine, i.e. a routine that can span several frames, note coroutines needs to be of the type IEnumerator
        StartCoroutine(DeletePlayer("karsten"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Tests the HTTP connection
    private IEnumerator CreatePlayer(string name, int score)
    {
        //Sends a Web Request - HTTP Get "/createPlayer"
        UnityWebRequest request = UnityWebRequest.Get($"{URI}createPlayer?name={name}&score={score}");

        //Handles the send Request
        var handler = request.SendWebRequest();

        //Tracks the responseTime
        float responseTime = 0.0f;

        //While awaiting a response
        while (!handler.isDone)
        {
            //Adds the time since the last frame
            responseTime += Time.deltaTime;

            //Cancels the request if the responseTime exceeds 10 seconds
            if (responseTime > 10.0f)
            {
                Debug.Log("Cancel request - ResponseTime exceeds 5 seconds");
                break;
            }

            //Wait for the next frame and continue execution from this line
            yield return null;
        }

        //If the Request succesfully received a Response
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received response: ");
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error - No response received");
        }
    }

    private IEnumerator ReadPlayer(string name)
    {
        //Sends a Web Request - HTTP Get "/createPlayer"
        UnityWebRequest request = UnityWebRequest.Get($"{URI}readPlayer?name={name}");

        //Handles the send Request
        var handler = request.SendWebRequest();

        //Tracks the responseTime
        float responseTime = 0.0f;

        //While awaiting a response
        while (!handler.isDone)
        {
            //Adds the time since the last frame
            responseTime += Time.deltaTime;

            //Cancels the request if the responseTime exceeds 10 seconds
            if (responseTime > 10.0f)
            {
                Debug.Log("Cancel request - ResponseTime exceeds 5 seconds");
                break;
            }

            //Wait for the next frame and continue execution from this line
            yield return null;
        }

        //If the Request succesfully received a Response
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received response: ");
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error - No response received");
        }
    }
    private IEnumerator UpdatePlayer(string name, int score)
{
    //Sends a Web Request - HTTP Get "/createPlayer"
    UnityWebRequest request = UnityWebRequest.Get($"{URI}updatePlayer?name={name}&score={score}");

    //Handles the send Request
    var handler = request.SendWebRequest();

    //Tracks the responseTime
    float responseTime = 0.0f;

    //While awaiting a response
    while (!handler.isDone)
    {
        //Adds the time since the last frame
        responseTime += Time.deltaTime;

        //Cancels the request if the responseTime exceeds 10 seconds
        if (responseTime > 10.0f)
        {
            Debug.Log("Cancel request - ResponseTime exceeds 5 seconds");
            break;
        }

        //Wait for the next frame and continue execution from this line
        yield return null;
    }

    //If the Request succesfully received a Response
    if (request.result == UnityWebRequest.Result.Success)
    {
        Debug.Log("Received response: ");
        Debug.Log(request.downloadHandler.text);
    }
    else
    {
        Debug.Log("Error - No response received");
    }
}

    private IEnumerator DeletePlayer(string name)
    {
        //Sends a Web Request - HTTP Get "/createPlayer"
        UnityWebRequest request = UnityWebRequest.Get($"{URI}deletePlayer?name={name}");

        //Handles the send Request
        var handler = request.SendWebRequest();

        //Tracks the responseTime
        float responseTime = 0.0f;

        //While awaiting a response
        while (!handler.isDone)
        {
            //Adds the time since the last frame
            responseTime += Time.deltaTime;

            //Cancels the request if the responseTime exceeds 10 seconds
            if (responseTime > 10.0f)
            {
                Debug.Log("Cancel request - ResponseTime exceeds 5 seconds");
                break;
            }

            //Wait for the next frame and continue execution from this line
            yield return null;
        }

        //If the Request succesfully received a Response
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received response: ");
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error - No response received");
        }
    }
}
