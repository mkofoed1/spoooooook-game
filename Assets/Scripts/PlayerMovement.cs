using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class PlayerMovement : NetworkBehaviour
{
    // Player References
    public string playername;
    public int score;

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

    // Ghost References
    [SerializeField] public GameObject ghost;
    [SerializeField] private GameObject ghost1;
    [SerializeField] private GameObject ghost2;
    [SerializeField] private GameObject ghost3;
    [SerializeField] private GameObject ghost4;

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
            ghost = GameObject.Find("Ghost1");
            ghost.GetComponent<Observer>().player = this.transform;
            ghost1 = GameObject.Find("Ghost2");
            ghost1.GetComponent<Observer>().player = this.gameObject.transform;
            ghost2 = GameObject.Find("Ghost3");
            ghost2.GetComponent<Observer>().player = this.gameObject.transform;
            ghost3 = GameObject.Find("Ghost4");
            ghost3.GetComponent<Observer>().player = this.gameObject.transform;
            ghost4 = GameObject.Find("Ghost5");
            ghost4.GetComponent<Observer>().player = this.gameObject.transform;
        }
    }


    void FixedUpdate ()
    {
        if(!isLocalPlayer)
        {
            return;
        }
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
        }
    }
}