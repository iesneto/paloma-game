using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamob
{
    public class CameraFollow : MonoBehaviour
    {
        public GameObject target;
        public Vector3 offset;
        private Vector3 velocity = Vector3.zero;
        public float dampTime = 0.2f;
        public float rotateDampTime = 0.2f;
        private Vector3 lookTarget;
        public Vector3 lookOffset;
        [SerializeField] private bool shock;

        public void SetPlayer(GameObject player)
        {
            target = player;
        }

        public void Shock(bool _shock)
        {
            shock = _shock;
        }


        private void LateUpdate()
        {
            if (target == null || shock) return;
            //transform.position = target.transform.position + offset;



            Vector3 destination = target.transform.position + offset;



            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

            lookTarget = target.transform.position + lookOffset;


            //Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            Quaternion targetRotation = Quaternion.LookRotation(lookTarget - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateDampTime);
        }

    }
}
