using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    public float dampTime = 0.2f;
    public float rotateDampTime = 0.2f;

    public void SetPlayer(GameObject player)
    {
        target = player;
    }


    private void LateUpdate()
    {
        if (target == null) return;
        //transform.position = target.transform.position + offset;



        Vector3 destination = target.transform.position + offset;


        

        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

        
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateDampTime);
    }

}
