using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPositioning : MonoBehaviour
{
    [SerializeField] private float posY;
    [SerializeField] private float rayLength;

    private void Start()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if(hit.collider.gameObject.tag == "Floor")
            {
                float yMundo = transform.position.y - hit.distance; 
                // if(hit.distance <= posY)
                //{
                transform.position = new Vector3(transform.position.x, posY + yMundo, transform.position.z);
                //}
                

            }
        }
    }
}
