using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    private float m_YawRotationSpeed = 100;
    private float m_PitchRotationalSpeed = 100;
    private float m_MinPitch = -60.0f;
    private float m_MaxPitch = 30.0f;
    public float m_OffsetOnCollision = 2;
    public Transform m_LookAt;
    public float m_DistanceToLookAt = 15;
    public LayerMask m_RaycastLayerMask;
    private Vector3 m_RestartPosition;
    private Vector3 detrasPos;
    private Vector3 lastlookAtPosition;
    private float m_timer = 0;
    private bool collisioning = false;
    private Vector3 posCollision;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        float mouseAxisX = 0;
        float mouseAxisY = 0;

        mouseAxisX = Input.GetAxis("Mouse X");
        mouseAxisY = Input.GetAxis("Mouse Y");

        Vector3 l_Direction = m_LookAt.position - transform.position;
        float l_Distance = l_Direction.magnitude;
        //..
        //..
        Vector3 l_DesiredPosition = transform.position;

        if ((mouseAxisX > 0.01f || mouseAxisX < -0.01f || mouseAxisY > 0.01f || mouseAxisY < -0.01f))
        {
            Vector3 l_EulerAngles = transform.eulerAngles;
            float l_Yaw = (l_EulerAngles.y + 180.0f);
            float l_Pitch = l_EulerAngles.x;

            l_Yaw += m_YawRotationSpeed * mouseAxisX * Time.deltaTime;
            l_Yaw *= Mathf.Deg2Rad;
            if (l_Pitch > 180.0f)
                l_Pitch -= 360.0f;
            l_Pitch += m_PitchRotationalSpeed * (-mouseAxisY) * Time.deltaTime;
            l_Pitch = Mathf.Clamp(l_Pitch, m_MinPitch, m_MaxPitch);
            l_Pitch *= Mathf.Deg2Rad;
            l_DesiredPosition = m_LookAt.position + new Vector3(Mathf.Sin(l_Yaw) * Mathf.Cos(l_Pitch) * l_Distance, Mathf.Sin(l_Pitch) * l_Distance, Mathf.Cos(l_Yaw) * Mathf.Cos(l_Pitch) * l_Distance);
            l_Direction = m_LookAt.position - l_DesiredPosition;
        }
        l_Direction /= l_Distance;


        l_DesiredPosition = m_LookAt.position - l_Direction * m_DistanceToLookAt;
        l_Distance = m_DistanceToLookAt;
        

        RaycastHit l_RaycastHit;
        Ray l_Ray = new Ray(m_LookAt.position, -l_Direction);
        if (Physics.Raycast(l_Ray, out l_RaycastHit, l_Distance, m_RaycastLayerMask.value))
            l_DesiredPosition = l_RaycastHit.point + l_Direction * m_OffsetOnCollision;

        transform.forward = l_Direction;
        transform.position = l_DesiredPosition;
    }
}

