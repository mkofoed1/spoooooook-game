using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
using TMPro;

public class PlayerMovement : NetworkBehaviour
{
    // Player References
    [SyncVar]
    public string playername;
    [SyncVar]
    public int score;
    public GameObject nameplate;

    // Movement References
    public float turnSpeed = 20f;    
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    // Animation & Audio
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;

    // Camera
    [SerializeField] private CinemachineVirtualCamera cam;

    void Start ()
    {
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource> ();

        if(isLocalPlayer)
        {
            cam = CinemachineVirtualCamera.FindObjectOfType<CinemachineVirtualCamera>();
            cam.LookAt = this.gameObject.transform;
            cam.Follow = this.gameObject.transform;
        }

        nameplate = gameObject.transform.GetChild(2).gameObject;
        UpdateNamePlate();
    }


    void UpdateNamePlate()
    {
        nameplate.GetComponentInChildren<TextMeshProUGUI>().text = playername + "\n" + "Score: " + score;
    }
    
    void FixedUpdate ()
    {


        if(!isLocalPlayer)
        {
            if (nameplate != null)
            nameplate.transform.rotation = Quaternion.LookRotation(cam.transform.position - transform.position) * Quaternion.Euler(0, 180, 0);
            return;
        }

        // Nameplate Rotate towards Camera
        nameplate.transform.rotation = Quaternion.LookRotation(cam.transform.position - transform.position) * Quaternion.Euler(0, 180, 0);
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);
        
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop ();
        }

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
    }

    void OnAnimatorMove ()
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation (m_Rotation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "coin")
        {
            score++;
            StartCoroutine(RequestHandler.Instance.UpdatePlayer(playername, score));
            UpdateNamePlate();
        }
    }
}