using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    public Camera m_Camera;

    void Start()
    {
        FindCamera();
    }

    void FindCamera()
    {
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");

        if (cam != null) m_Camera = cam.GetComponent<Camera>();
    }

    void Update()
    {
        if (m_Camera != null)
            transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
        else
        {
            FindCamera();
        }
    }
}
