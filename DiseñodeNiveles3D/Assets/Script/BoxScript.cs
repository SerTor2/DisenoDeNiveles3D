using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    public bool gravity = false;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private MeshRenderer renderer;
    private bool destroyed = false;
    private bool saved = false;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        renderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        if (gravity)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        else
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DestroyBox()
    {
        renderer.enabled = false;
        boxCollider.enabled = false;
        destroyed = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(collision.gameObject.transform.position.y < gameObject.transform.position.y + 1)
            {
                DestroyBox();
            }

        }
    }

    public void SaveBox()
    {
        if (destroyed)
            saved = true;
    }


    public void Respawn()
    {
        if(!saved)
        {
            renderer.enabled = true;
            boxCollider.enabled = true;
            destroyed = false;
        }
    }
}
