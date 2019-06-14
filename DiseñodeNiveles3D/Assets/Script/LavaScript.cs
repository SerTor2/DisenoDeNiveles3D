using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour
{
    public List<float> alturas;
    private GameObject player;
    private PlayerScript playerScript;
    private Vector3 toGo = Vector3.zero;
    private float lastAltura;
    private Vector3 savedPosition;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
        lastAltura = player.transform.position.y;
        savedPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(toGo.magnitude != 0)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, toGo, 2 * Time.deltaTime);
            if ((gameObject.transform.position - toGo).magnitude <= 0.02f)
                toGo = Vector3.zero;
        }
    }

    public void Respawn()
    {
        gameObject.transform.position = savedPosition;
        toGo = Vector3.zero;
    }

    public void checkPoint()
    {
        savedPosition = gameObject.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerScript>().Respawn();
        }
    }

    public void NextPoint()
    {
        if (player.transform.position.y >= gameObject.transform.position.y + 7f)
        {
            lastAltura = player.transform.position.y;

            foreach (float f in alturas)
            {
                if (player.transform.position.y > f + 3f && f > gameObject.transform.position.y)
                    toGo = new Vector3(gameObject.transform.position.x, f, gameObject.transform.position.z);

            }
        }
    }
}
