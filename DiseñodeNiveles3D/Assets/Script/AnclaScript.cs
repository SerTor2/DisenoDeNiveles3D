using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnclaScript : MonoBehaviour
{
    public Transform posToGo;
    private Vector3 startPosition;
    private float currentTime = 0;
    private bool go = true;
    private Vector3 differencePosition = new Vector3();
    private Vector3 lastPosition = new Vector3();
    private PlayerScript player;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        lastPosition = transform.position;
        if (go)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= 2)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, posToGo.position, 50 * Time.deltaTime);
                if((gameObject.transform.position - posToGo.position).magnitude <= 0.5)
                {
                    go = false;
                    currentTime = 0;
                }

            }
        }
        else
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 1.5)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, startPosition, 10 * Time.deltaTime);
                differencePosition = transform.position - lastPosition;
                if (player != null)
                {
                    player.MovePlayerWithAncla(differencePosition);
                }
                if ((gameObject.transform.position - startPosition).magnitude <= 0.5)
                {
                    go = true;
                    if (player != null)
                    {
                        player.gameObject.transform.parent = null;
                        player = null;
                    }
                    currentTime = 0;
                }

            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.gameObject.GetComponent<PlayerScript>().Respawn();
    }

    public void SetPlayer(PlayerScript _player)
    {
        player = _player;
    }
}
