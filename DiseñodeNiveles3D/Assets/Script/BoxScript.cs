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
    public float salto = 0.6f;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.position;
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
        if (gravity)
        {
            rb.useGravity = false;
            rb.isKinematic = false;
        }
        else
        {
            rb.useGravity = false;
            rb.isKinematic = false;
        }
        renderer.enabled = false;
        boxCollider.enabled = false;
        destroyed = true;
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
            gameObject.transform.position = startPos;
            renderer.enabled = true;
            boxCollider.enabled = true;
            destroyed = false;
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
    }
}
