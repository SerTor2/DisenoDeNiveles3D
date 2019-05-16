using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private CharacterController characterController;
    private Vector3 movement;
    private float m_Yaw;
    private float m_Pitch;
    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    private float speed = 20;
    private float speedJump = 150;
    private float verticalSpeed;
    private bool onGround = false;
    private SphereCollider tiggerGiro;
    private float timeGiro = 0.2f;
    private float currentTime = 0;
    private bool girando = false;
    public LayerMask m_RaycastLayerMask;
    private Animation animation;
    // Start is called before the first frame update
    void Awake()
    {
        animation = GetComponent<Animation>();
        tiggerGiro = GetComponent<SphereCollider>();
        characterController = GetComponent<CharacterController>();
        m_Yaw = transform.rotation.eulerAngles.y;
        m_Pitch = transform.localRotation.eulerAngles.x;
        
    }

    // Update is called once per frame
    void Update()
    {

        #region movement

        float l_YawInRadians = m_Yaw * Mathf.Deg2Rad;
        float l_Yaw90InRadians = (m_Yaw + 90.0f) * Mathf.Deg2Rad;

        movement = new Vector3(0, 0, 0);
        Vector3 l_Forward = new Vector3(Mathf.Sin(l_YawInRadians), 0.0f, Mathf.Cos(l_YawInRadians));
        Vector3 l_Right = new Vector3(Mathf.Sin(l_Yaw90InRadians), 0.0f, Mathf.Cos(l_Yaw90InRadians));

        if (Input.GetKey(m_UpKeyCode))
        {
            movement = l_Forward;
        }
        else if (Input.GetKey(m_DownKeyCode))
            movement = -l_Forward;



        if (Input.GetKey(m_RightKeyCode))
            movement += l_Right;
        else if (Input.GetKey(m_LeftKeyCode))
            movement -= l_Right;
        ///*
        if (Input.GetKey(m_JumpKeyCode))
            movement += Vector3.up;
        else if (Input.GetKey(KeyCode.LeftShift))
            movement += Vector3.down;
        //*/
        movement.Normalize();

        movement = movement * Time.deltaTime * speed;
        //...

        CollisionFlags l_CollisionFlags = characterController.Move(movement);

        /*verticalSpeed += Physics.gravity.y * Time.deltaTime * 5f;

        if (onGround)
            verticalSpeed = 0;

        if (onGround && Input.GetKey(m_JumpKeyCode))
        {
            Jump();
        }
        else
        {
            movement = Vector3.zero;
            movement.y = verticalSpeed * Time.deltaTime;

            IsInPlatform();
            l_CollisionFlags = characterController.Move(movement);
        }
        if ((l_CollisionFlags & CollisionFlags.Below) != 0 && verticalSpeed <= 0)
        {
            onGround = true;
            verticalSpeed = 0.0f;
        }
        else
            onGround = false;

        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && verticalSpeed > 0.0f)
        {
            verticalSpeed = 0.0f;
        }*/
        #endregion

        #region giro
        if (Input.GetMouseButtonDown(0) && !girando)
        {
            animation.Play();
            girando = true;
            tiggerGiro.enabled = girando;
        }

        if(girando)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= timeGiro)
            {
                girando = false;
                tiggerGiro.enabled = girando;
                currentTime = 0;
            }
        }

        #endregion


    }


    public void Jump(float _multiply = 1)
    {
        onGround = false;
        verticalSpeed = 0;
        movement = Vector3.zero;
        verticalSpeed += Physics.gravity.y * Time.deltaTime * -speedJump * _multiply;
        movement.y = verticalSpeed * Time.deltaTime;
        CollisionFlags l_CollisionFlags = characterController.Move(movement);
    }

    public void MovePlayerWithAncla(Vector3 _move)
    {
        verticalSpeed = 0;
        CollisionFlags collisionflags = characterController.Move(_move);
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Box")
        {
            if (hit.gameObject.transform.position.y + 1 < gameObject.transform.position.y)
            {
                Jump(1.1f);
                Destroy(hit.gameObject);
            }
            else if(hit.gameObject.transform.position.y -1 > gameObject.transform.position.y)
            {
                Destroy(hit.gameObject);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(girando)
        {
            if (other.gameObject.tag == "Box")
                Destroy(other.gameObject);
        }
    }

    private void IsInPlatform()
    {
        RaycastHit l_RaycastHit;
        Ray l_Ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(l_Ray, out l_RaycastHit, 2f, m_RaycastLayerMask.value))
        {
            if (l_RaycastHit.collider.gameObject.tag == "Ancla")
            {
                onGround = true;
                if (l_RaycastHit.collider.gameObject.GetComponent<AnclaScript>() != null)
                    l_RaycastHit.collider.gameObject.GetComponent<AnclaScript>().SetPlayer(this);
                gameObject.transform.parent = l_RaycastHit.collider.gameObject.transform;
                
            }
        }
        else
        {
            if(gameObject.transform.parent != null)
            {
                if(gameObject.transform.parent.gameObject.GetComponent<AnclaScript>() != null)
                {
                    gameObject.transform.parent.gameObject.GetComponent<AnclaScript>().SetPlayer(null);
                    gameObject.transform.parent = null;
                }
            }
        }

    }

}
