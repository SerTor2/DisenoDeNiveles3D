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
        if (toGo.magnitude == 0)
        {
            if (player.transform.position.y >= gameObject.transform.position.y + 7f)
            {
                lastAltura = player.transform.position.y;

                foreach (float f in alturas)
                {
                    if (player.transform.position.y > f + 0.6f && f > gameObject.transform.position.y && playerScript.onGround && player.transform.parent == null)
                        toGo = new Vector3(gameObject.transform.position.x, f, gameObject.transform.position.z);

                }
            }
        }
        else
        {
            if (player.transform.position.y >= toGo.y + 7f)
            {
                lastAltura = player.transform.position.y;

                foreach (float f in alturas)
                {
                    if (player.transform.position.y > f + 0.6f && f > gameObject.transform.position.y && playerScript.onGround && player.transform.parent == null)
                        toGo = new Vector3(gameObject.transform.position.x, f, gameObject.transform.position.z);

                }
            }
        }

        if (player.transform.position.y <= lastAltura - 2f)
            toGo = Vector3.zero;

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
    }

    public void checkPoint()
    {
        savedPosition = gameObject.transform.position;
    }
}
