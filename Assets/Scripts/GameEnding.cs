using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameEnding : NetworkBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public AudioSource exitAudio;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource caughtAudio;

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;
    bool m_HasAudioPlayed;
    
    void Start()
    {
        if(isLocalPlayer)
        {
            player = this.gameObject;
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
            NetworkIdentity networkIdentity = other.gameObject.GetComponent<NetworkIdentity>();


        }
    }

    [Server]
    public void CaughtPlayer (NetworkIdentity networkIdentity)
    {
        TargetEndLevel(networkIdentity.connectionToClient);
    }

    void Update ()
    {
        if (m_IsPlayerAtExit)
        {
            StartCoroutine(EndLevel (exitBackgroundImageCanvasGroup, false, exitAudio));
        }
        else if (m_IsPlayerCaught)
        {
            StartCoroutine(EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio));
        }
    }

    [TargetRpc]
    void TargetEndLevel(NetworkConnection target)
    {
        StartCoroutine(EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio));
    }
    
    [Client]
    IEnumerator EndLevel (CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }

        while(!(m_Timer > fadeDuration + displayImageDuration))
        {
            m_Timer += Time.deltaTime;
            imageCanvasGroup.alpha = m_Timer / fadeDuration;
            yield return new WaitForEndOfFrame();
        }
        

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene (0);
                //StartCoroutine(RequestHandler.Instance.DeletePlayer(spiller.name));
            }
            else
            {
                Application.Quit ();
                //StartCoroutine(RequestHandler.Instance.DeletePlayer(spiller.name));
            }
        }
    }
}
