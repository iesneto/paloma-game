using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CowBehavior : MonoBehaviour
{
    //public bool Grabbed { set; get; }
    public CowData cowData;
    private bool grabbed;
    private Rigidbody rb;
    private Animator anim;
    private float walkChance;
    private bool walk;
    private float timeIdle;
    private bool idle;
    private bool waitingCoroutine;
    private float movementSpeed;
    private float rotationSpeed;
    private Vector2 randomDir;
    private Vector3 destination;
    private Vector2 distanceLimits;
    private bool backFromWalk;
    //private bool grounded;
    public float destinyDistance;
    private PlayerBehavior player;


    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        walkChance = 0.8f;
        rb.mass = cowData.mass;
        movementSpeed = cowData.movementSpeed;
        rotationSpeed = movementSpeed;
        distanceLimits = new Vector2(15, 20);

        //StartCoroutine(ResetCowFromGrabbed());
        //GotoIdle();
    }

    private void GotoIdle()
    {
        idle = true;

        StartCoroutine("ChangeIdleAnimation");
    }

    private void GotoWalk()
    {
        StartCoroutine("Walking");
    }

    IEnumerator ChangeIdleAnimation()
    {
        waitingCoroutine = true;

        while (!grabbed && !walk)
        {
            if (idle)
            {
                idle = false;
                float canWalk = Random.value;
                if (canWalk <= walkChance && !backFromWalk)
                {
                    walk = true;

                    //StartCoroutine("Walking");
                    GotoWalk();

                }
                else
                {
                    backFromWalk = false;
                    int chance = Random.Range(0, 3);
                    switch (chance)
                    {
                        case 0:
                            anim.SetBool("observa", true);
                            break;
                        case 1:
                            anim.SetBool("pasta", true);
                            break;
                        case 2:
                            anim.SetBool("muge", true);
                            break;
                        default: break;
                    }


                }

            }
            timeIdle = Random.Range(10f, 12f);
            yield return new WaitForSeconds(timeIdle);
        }
        //yield return null;
        waitingCoroutine = false;
    }

    IEnumerator Walking()
    {
        //randomDir = Random.insideUnitCircle;
        //destination = transform.position + new Vector3(randomDir.x * Random.Range(distanceLimits.x, distanceLimits.y), 0, randomDir.y * Random.Range(distanceLimits.x, distanceLimits.y));
        destination = GetDestinationPoint();

        Vector3 dirDiff;// = (destination - transform.position).normalized;
        Vector3 direction;// = new Vector3(dirDiff.x, 0, dirDiff.z);

        //for (float t = 0; t < 1f; t += Time.deltaTime)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(direction);
        //    gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
        //    yield return null;
        //}


        anim.SetBool("walk", true);
        anim.speed = movementSpeed;
        while (!grabbed && walk)
        {
            dirDiff = (destination - transform.position).normalized;
            direction = new Vector3(dirDiff.x, 0, dirDiff.z);
            // Rotate the cube by converting the angles into a quaternion.
            //Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
            // Dampen towards the target rotation
            //gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            // gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            //gameObject.transform.Translate(direction * movementSpeed * Time.deltaTime);
            gameObject.transform.position += (gameObject.transform.forward * movementSpeed * Time.deltaTime);
            //gameObject.transform.position = Vector3.Lerp(transform.position, destination, movementSpeed * Time.deltaTime);
            destinyDistance = Vector3.Distance(transform.position, destination);

            //if (Vector3.Distance(transform.position, destination) <= 1.5f)
            if (destinyDistance <= 1.5f)
            {
                walk = false;
                EndAnimation(3);
                backFromWalk = true;
                if (!waitingCoroutine)
                    GotoIdle();
            }
            yield return null;

        }
        anim.speed = 1;


    }

    private Vector3 GetDestinationPoint()
    {
        RaycastHit hit;
        bool blocked = true;
        Vector3 dest = new Vector3();
        while (blocked)
        {
            randomDir = Random.insideUnitCircle;
            dest = transform.position + new Vector3(randomDir.x * Random.Range(distanceLimits.x, distanceLimits.y), 0, randomDir.y * Random.Range(distanceLimits.x, distanceLimits.y));
            float sightDistance = (dest - transform.position).magnitude;
            Vector3 dirDiff = (dest - transform.position).normalized;
            Vector3 direction = new Vector3(dirDiff.x, 0, dirDiff.z);
            //float calculo = (((float)i / 2) - (((float)numRays - 1) / 4)) * sightFOV;
            //Vector3 forward = sight.transform.TransformDirection(Vector3.forward) * sightDistance;
            //Vector3 forward = sight.transform.forward * sightDistance;
            //Vector3 bias = sight.transform.right * calculo;
            //Vector3 direction = forward + bias;
            Ray ray = new Ray(transform.position, direction);
            Debug.DrawRay(transform.position, dest - transform.position, Color.blue, 5);
            if (!Physics.Raycast(ray, out hit, sightDistance))
            {


                blocked = false;
            }
            //else
            //{
            //    if (hit.collider.tag == "Bound")
            //    {

            //        //sighted = true;
            //        //player = hit.collider.gameObject;
            //        ////canAttack = true;
            //        //currentState = state.attack;
            //        //animator.SetBool("walk", true);
            //    }

            //}
        }
        return dest;
    }


    public void EndAnimation(int i)
    {

        switch (i)
        {
            case 0:
                anim.SetBool("observa", false);
                break;
            case 1:
                anim.SetBool("pasta", false);
                break;
            case 2:
                anim.SetBool("muge", false);
                break;
            case 3:
                anim.SetBool("walk", false);
                break;
            default: break;
        }
        idle = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            //grounded = true;
            //if (grabbed)
            //{

            //    // update 18/09/2022 - Ivo Seitenfus
            //    // moving outside this if
            //    //StartCoroutine("ResetCowFromGrabbed");
            //}

            // executing every time detects this collision
            StartCoroutine("ResetCowFromGrabbed");


        }
    }

    public void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.tag == "Floor")
        //{
        //    grounded = false;

        //}
    }

    public void OnGrabbed(PlayerBehavior _player)
    {
        if (!grabbed)
        {

            grabbed = true;
            walk = false;

            //Update 19/09/2022 - Ivo Seitenfus
            // New grab animation
            //anim.SetBool("levita", true);
            
            anim.SetBool("observa", false);
            anim.SetBool("pasta", false);
            anim.SetBool("muge", false);
            anim.SetBool("walk", false);
            anim.SetBool("grab", true);
            player = _player;
        }

    }

    public void StartPlayerGrabAnimation()
    {
        player.StartGrabAnimation();
    }


    IEnumerator ResetCowFromGrabbed()
    {
        yield return new WaitForSeconds(1f);
        //Update 19/09/2022 - Ivo Seitenfus
        //anim.SetBool("levita", false);
        anim.SetBool("grab", false);
        grabbed = false;
        idle = true;
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = new Vector3(0f, 0f, 0f);
        for (float i = 0; i <= 1; i += Time.deltaTime * 2)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, rotationSpeed);
        }

        if (!waitingCoroutine)
            GotoIdle();



    }


    public int GetReward()
    {
        return cowData.reward;
    }

    public void OnGrab()
    {
        if(player != null)
        {
            player.GetCow(this.gameObject);
        }

    }
}
