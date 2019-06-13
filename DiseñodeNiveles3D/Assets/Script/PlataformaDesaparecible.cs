using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaDesaparecible : MonoBehaviour
{
    private bool dentro = false;
    private float currentTime = 0;
    public BoxCollider boxCollider;
    private MeshRenderer renderer;
    private Color actualColor;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        actualColor = renderer.material.color;
    }
    // Update is called once per frame
    void Update()
    {
        if(dentro)
        {
            currentTime += Time.deltaTime;
            renderer.material.color = new Color(actualColor.r, actualColor.g, actualColor.b, (1 - currentTime) / 1f);
            if (currentTime >= 1f)
            {
                boxCollider.enabled = false;
                renderer.enabled = false;
            }

            if(currentTime >= 1.5f)
            {
                renderer.material.color = actualColor;
                currentTime = 0;
                dentro = false;
                renderer.enabled = true;
                boxCollider.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Fuera();
        }
    }


    public void Dentro()
    {
        dentro = true;
    }

    public void Fuera()
    {
        renderer.material.color = actualColor;
        currentTime = 0;
        dentro = false;
        renderer.enabled = true;
        boxCollider.enabled = true;
    }
}
