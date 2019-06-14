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
    private float speed = 13;
    private float speedJump = 20;
    public float verticalSpeed;
    public bool onGround = false;
    private SphereCollider tiggerGiro;
    private float timeGiro = 0.35f;
    private float currentTime = 0;
    public bool girando = false;
    public LayerMask m_RaycastLayerMask;
    private int lifes = 3;
    private Vector3 RespawnPosition;
    private List<PatrolEnemie> enemiesPatrol = new List<PatrolEnemie>();
    private List<TurretEnemie> enemiesTurret = new List<TurretEnemie>();
    private List<BoxScript> boxes = new List<BoxScript>();
    public GameObject myCamera;
    private GameObject[] monedas;
    public Animator animator;
    public LavaScript lava;
    // Start is called before the first frame update
    void Awake()
    {
        tiggerGiro = GetComponent<SphereCollider>();
        characterController = GetComponent<CharacterController>();
        m_Yaw = transform.rotation.eulerAngles.y;
        m_Pitch = transform.localRotation.eulerAngles.x;
        RespawnPosition = gameObject.transform.position;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GameObject[] enemiesP = GameObject.FindGameObjectsWithTag("PatrolEnemie");
        foreach(GameObject go in enemiesP)
        {
            enemiesPatrol.Add(go.GetComponent<PatrolEnemie>());
        }

        GameObject[] enemiesT = GameObject.FindGameObjectsWithTag("TurretEnemie");
        foreach (GameObject go in enemiesT)
        {
            enemiesTurret.Add(go.GetComponent<TurretEnemie>());
        }

        GameObject[] _boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach (GameObject go in _boxes)
        {
            boxes.Add(go.GetComponent<BoxScript>());
        }

        monedas = GameObject.FindGameObjectsWithTag("Moneda");
    }

    // Update is called once per frame
    void Update()
    {

        #region movement

        float l_YawInRadians = m_Yaw * Mathf.Deg2Rad;
        float l_Yaw90InRadians = (m_Yaw + 90.0f) * Mathf.Deg2Rad;

        movement = new Vector3(0, 0, 0);
        Vector3 l_Forward = myCamera.transform.forward;
        Vector3 l_Right = myCamera.transform.right;

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

        movement = new Vector3(movement.x, 0, movement.z);
        movement.Normalize();

        movement = movement * Time.deltaTime * speed;
        //...
        if (movement.x != 0 || movement.z != 0)
        {
            animator.SetBool("Movement", true);
            gameObject.transform.forward = new Vector3(movement.x, 0, movement.z);
        }
        else
        {
            animator.SetBool("Movement", false);
        }
        if (gameObject.transform.parent == null)
            gameObject.transform.localScale = Vector3.one;
        else
            gameObject.transform.localScale = new Vector3(0.25f,1,1);

        CollisionFlags l_CollisionFlags = characterController.Move(movement);

        verticalSpeed += Physics.gravity.y * Time.deltaTime * 5f;

        if (onGround)
            verticalSpeed = 0;
        else
            animator.SetBool("OnGround", onGround);


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
            animator.SetBool("OnGround", onGround);
            verticalSpeed = 0.0f;
        }
        else
            onGround = false;

        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && verticalSpeed > 0.0f)
        {
            verticalSpeed = 0.0f;
        }
        #endregion

        #region giro
        if (Input.GetMouseButtonDown(0) && !girando)
        {
            animator.SetTrigger("Giro");
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
        animator.SetTrigger("Jump");
        onGround = false;
        verticalSpeed = 0;
        movement = Vector3.zero;
        verticalSpeed += speedJump * _multiply;
        movement.y = verticalSpeed * Time.deltaTime;
    }

    public void MovePlayerWithAncla(Vector3 _move)
    {
        verticalSpeed = 0;
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Box")
        {
            if (hit.gameObject.transform.position.y + 1 < gameObject.transform.position.y)
            {
                Jump(hit.gameObject.GetComponent<BoxScript>().salto);
                hit.gameObject.GetComponent<BoxScript>().DestroyBox();
            }
            else if(hit.gameObject.transform.position.y -1 > gameObject.transform.position.y)
            {
                hit.gameObject.GetComponent<BoxScript>().DestroyBox();
            }
        }
        if(hit.gameObject.tag == "Desaparecible")
        {
            hit.gameObject.GetComponent<PlataformaDesaparecible>().Dentro();
        }

        if (hit.gameObject.tag == "CheckPoint")
        {
            if (hit.gameObject.transform.position.y + 1 < gameObject.transform.position.y)
            {
                hit.gameObject.GetComponent<CheckPointScript>().Change();
                foreach (BoxScript box in boxes)
                    box.SaveBox();
                RespawnPosition = hit.gameObject.transform.position + hit.gameObject.transform.forward * 2;
                lava.checkPoint();

            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(girando)
        {
            if (other.gameObject.tag == "Box")
                other.gameObject.GetComponent<BoxScript>().DestroyBox();
        }

        if (other.gameObject.tag == "Moneda")
            other.gameObject.SetActive(false);

        if(other.gameObject.tag == "LifeUp")
        {
            Destroy(other.gameObject);
            ModifieLife(1);
        }

        if(other.gameObject.tag == "CheckPoint" && girando)
        {
            other.gameObject.GetComponent<CheckPointScript>().Change();
            foreach (BoxScript box in boxes)
                box.SaveBox();
            RespawnPosition = other.gameObject.transform.position + other.gameObject.transform.forward * 2;
            lava.checkPoint();
        }

        if(other.gameObject.tag == "NextPlane")
        {
            lava.NextPoint();
        }
    }

    private void IsInPlatform()
    {
        RaycastHit l_RaycastHit;
        Ray l_Ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(l_Ray, out l_RaycastHit, 1.5f, m_RaycastLayerMask.value))
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

    public void Respawn()
    {
        lava.Respawn();
        ModifieLife(-1);
        characterController.enabled = false;
        gameObject.transform.position = RespawnPosition;
        characterController.enabled = true;

        foreach (GameObject go in monedas)
            go.SetActive(true);

        foreach (PatrolEnemie patrol in enemiesPatrol)
            patrol.Respawn();

        foreach (TurretEnemie turret in enemiesTurret)
            turret.Respawn();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Bullet"))
            Destroy(go);

        foreach (BoxScript box in boxes)
            box.Respawn();

        myCamera.GetComponent<CameraScript>().Respawn();
    }

    public void ModifieLife(int _num)
    {
        lifes += _num;
    }

}
