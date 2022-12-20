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
            NetworkIdentity networkIdentity = other.gameObject.GetComponent<NetworkIdentity>();
            StartCoroutine(EndLevel(networkIdentity.connectionToClient, true, exitAudio));

        }
    }

    [Server]
    public void CaughtPlayer (NetworkIdentity networkIdentity)
    {
        TargetEndLevel(networkIdentity.connectionToClient);
    }

    [TargetRpc]
    void TargetEndLevel(NetworkConnection target)
    {
        
        StartCoroutine(EndLevel(target, false, caughtAudio));
        
    }
    
    [Client]
    IEnumerator EndLevel (NetworkConnection target, bool Win, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }
        CanvasGroup imageCanvasGroup = Win ? exitBackgroundImageCanvasGroup : caughtBackgroundImageCanvasGroup;

        while(!(m_Timer > fadeDuration + displayImageDuration))
        {
            m_Timer += Time.deltaTime;
            imageCanvasGroup.alpha = m_Timer / fadeDuration;
            yield return new WaitForEndOfFrame();
        }
        

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (Win)
            {

                Application.Quit();
                //StartCoroutine(RequestHandler.Instance.DeletePlayer(spiller.name));
            }
            else
            {

                target.Disconnect();
                
                //StartCoroutine(RequestHandler.Instance.DeletePlayer(spiller.name));
            }
        }
    }
}
