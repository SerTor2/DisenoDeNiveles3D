using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemie : MonoBehaviour
{
    private float currentTime = 0;
    private float cadencia = 1.5f;
    private GameObject player;
    public GameObject bulletPrefab;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if ((player.gameObject.transform.position - gameObject.transform.position).magnitude <= 15)
            {

                currentTime += Time.deltaTime;
                if(currentTime >= cadencia)
                {
                    animator.SetTrigger("Shoot");
                    Instantiate(bulletPrefab, gameObject.transform.position, bulletPrefab.transform.rotation).GetComponent<BulletScript>().SetForwrd((player.transform.position - gameObject.transform.position).normalized); ;
                    currentTime -= cadencia;
                }
            }
            else
                currentTime = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
            if (player.girando)
            {
                gameObject.SetActive(false);
            }
            else
            {
                player.Respawn();
            }
        }
    }

    public void Respawn()
    {
        currentTime = 0;
        gameObject.SetActive(true);
    }
}
