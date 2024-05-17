using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateAround : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float angles;
    [SerializeField] private Vector3 axis;

    private void Start()
    {
        if (target != null) StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            gameObject.transform.RotateAround(target.position, axis, angles);
            yield return null;
        }
        
    }
}
