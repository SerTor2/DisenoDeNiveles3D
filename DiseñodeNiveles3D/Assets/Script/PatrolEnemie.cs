using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemie : MonoBehaviour
{
    private bool go = true;
    public Vector3 toGo = Vector3.left;
    public float unitsToGo = 8;
    public float speed = 3f;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(go)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, startPos + toGo * unitsToGo, speed * Time.deltaTime );
            if ((gameObject.transform.position - (startPos + toGo * unitsToGo)).magnitude <= 0.05f)
                go = false;
        }
        else
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, startPos, speed * Time.deltaTime);
            if ((gameObject.transform.position - startPos).magnitude <= 0.05f)
                go = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
            if(player.girando)
            {
                gameObject.SetActive(false);
            }
            else
            {
                print("matar");
            }
        }
    }

    public void Respawn()
    {
        go = true;
        gameObject.transform.position = startPos;
        gameObject.SetActive(true);
    }
}
