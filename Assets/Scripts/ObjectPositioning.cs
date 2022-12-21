/*
 * Gamob Mobile Games - Ivo Seitenfus Neto
 * This script keeps the Player at a desired level above
 * ground while moving, doing smooth leveling when the ground 
 * is irregular
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gamob
{
    public class ObjectPositioning : MonoBehaviour
    {
        [SerializeField] private float posY;
        [SerializeField] private float worldY;
        [SerializeField] private float rayLength;
        [SerializeField] private float levelingSpeed;
        //[SerializeField] private float deacceleration;
        // [SerializeField] private float dampTime;
        // [SerializeField] private float objectPosition;

        private void Start()
        {
            PositionObject();
        }

        public void PositionObject()
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, rayLength))
            {
                if (hit.collider.gameObject.tag == "Floor")
                {
                    worldY = transform.position.y - hit.distance;

                    transform.position = new Vector3(transform.position.x, posY + worldY, transform.position.z);



                }
            }
        }

        public void Leveling()
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, rayLength))
            {
                if (hit.collider.gameObject.tag == "Floor")
                {
                    worldY = transform.position.y - hit.distance;
                    // Ivo Seitenfus
                    // levelingSpeed is used for tolerance pourposes
                    if (transform.position.y < posY + worldY - levelingSpeed) StartCoroutine("SmoothLevelingUp");
                    else if (transform.position.y > posY + worldY + levelingSpeed) StartCoroutine("SmoothLevelingDown");
                }


            }
        }

        IEnumerator SmoothLevelingUp()
        {

            while (transform.position.y < posY + worldY)
            {
                transform.position += new Vector3(0, levelingSpeed, 0);
                yield return null;
            }

        }

        IEnumerator SmoothLevelingDown()
        {

            while (transform.position.y > posY + worldY)
            {
                transform.position -= new Vector3(0, levelingSpeed, 0);
                yield return null;
            }

        }




    }
}