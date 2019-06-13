using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 forward = Vector3.zero;
    private bool changed = false;
    private float speed = 12;
    private float currentTime = 0;
    // Update is called once per frame
    void Update()
    {
        if(forward.magnitude != 0)
        {
            currentTime += Time.deltaTime;
            gameObject.transform.position += forward * speed * Time.deltaTime;
            if (currentTime >= 5)
                Destroy(gameObject);
        }
    }

    public void SetForwrd(Vector3 _forward)
    {
        forward = _forward;
    }

    private void SwitchForward()
    {
        if (!changed)
        {
            forward = -forward;
            changed = true;
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
                SwitchForward();
            }
            else
            {
                print("matar");
            }
        }
        else if (other.gameObject.tag == "TurretEnemie" && changed)
        {
            other.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if(other.gameObject.tag != "TurretEnemie")
            Destroy(gameObject);

    }
}
