using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPositioning : MonoBehaviour
{
    [SerializeField] private float posY;

    private void Start()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, 50))
        {
            if(hit.collider.gameObject.tag == "Floor")
            {
                if(hit.distance <= posY)
                {
                    transform.position += new Vector3(0, posY - hit.distance, 0);
                }
                
            }
        }
    }
}
