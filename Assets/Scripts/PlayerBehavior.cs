using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.UI;
using UnityEngine.ParticleSystemJobs;

public class PlayerBehavior : MonoBehaviour
{
    public float grabRate;
    [SerializeField] private PlayerAttributes playerAttributes;
    [SerializeField] private ObjectPositioning levelingScript;
    [SerializeField] private GameObject raio;
    [SerializeField] private ParticleSystem rayObjectBase;
    [SerializeField] private ParticleSystem rayObjectRays;
    [SerializeField] private ParticleSystem rayObjectTractor;
    [SerializeField] private ParticleSystem turret;
    [SerializeField] private Transform turretTransform;
    [SerializeField] private Vector3 turretDirection;
    [SerializeField] private float turretRotationSpeed;

    //[SerializeField] private float movementSpeed;
    [SerializeField] private float deacceleration;
    private Vector2 pressStartPosition;
    private Vector2 pressCurrentPosition;
    [SerializeField] private Vector3 direction;
    [SerializeField] private bool move;
    [SerializeField] private bool grab;
    [SerializeField] private float dampTime;
    [SerializeField] private float tiltAngle;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float tiltBackTime;
    [SerializeField] private List<GameObject> cowsInRay;
    //[SerializeField] private float rayForce;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private float grabDistance;
    [SerializeField] private long score;
    [SerializeField] private Transform model;
    [SerializeField] private float speedMagnitude;
    //[SerializeField] private float flyDistance;
    [SerializeField] private Animator animator;
    //public Text outputText;
    private Rigidbody rb;
    

    private Controls controls;
    

    private void Awake()
    {
        controls = new Controls();
        playerAttributes = GetComponent<PlayerAttributes>();
        playerAttributes.InitializeAttributes();

        levelingScript = GetComponent<ObjectPositioning>();

        raio.SetActive(false);
        
        

        rb = gameObject.GetComponent<Rigidbody>();
        
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.UI.Enable();
        controls.Player.MouseMove.Enable();
        controls.Player.MouseMove.started += OnTouchMove;
        controls.Player.MouseMove.performed += OnTouchMove;
        controls.Player.MouseMove.canceled += OnTouchMove;
        
        // controls.Player.Press.Enable();
        // controls.Player.PressRelease.Enable();
        //// controls.Player.Press.Enable();
        // controls.Player.Press.started += Press;
        // controls.Player.Press.performed += StartMove;
        // controls.Player.PressRelease.started += StopMove;
        //controls.Player.Hold.canceled += HoldCanceled;
        //controls.Player.Press.started += PressStart;
        //controls.Player.Press.performed += PressPerformed;
        // controls.Player.Press.canceled += PressCanceled;
        //controls.Player.Hold.performed += StartMove;
        //controls.Player.Hold.canceled += StopMove;
        //controls.Player.Press.performed += Press;
        //controls.Player.Press.canceled += StopMove;

    }

    private void OnDisable()
    {
        controls.Player.Disable();
        controls.UI.Disable();
        controls.Player.MouseMove.Disable();
        controls.Player.MouseMove.started -= OnTouchMove;
        controls.Player.MouseMove.performed -= OnTouchMove;
        controls.Player.MouseMove.canceled -= OnTouchMove;
        
        //controls.Player.Press.Disable();
        //controls.Player.PressRelease.Disable();
        ////controls.Player.Press.Disable();
        //controls.Player.Press.started -= Press;
        //controls.Player.Press.performed -= StartMove;
        //controls.Player.PressRelease.started -= StopMove;
        //controls.Player.Hold.canceled -= HoldCanceled;
        //controls.Player.Press.started -= PressStart;
        //controls.Player.Press.performed -= PressPerformed;
        //controls.Player.Press.canceled -= PressCanceled;
        //controls.Player.Hold.performed -= StartMove;
        //controls.Player.Hold.canceled -= StopMove;
        //controls.Player.Press.performed -= Press;
        //controls.Player.Press.canceled -= StopMove;
    }

    //private void Update() => Move();

    public void OnTouchMove(InputAction.CallbackContext context)
    {
        
        //update 19/09/2022 - Ivo Seitenfus
        // Automatic grab now executed by an coroutine
        //if(context.started)
        //{
            
        //    if (!move && (cowsInRay.Count > 0))
        //    {
        //        LevitateCow();
        //    }
        //}

        if (context.performed && !grab)
        {
            
            pressStartPosition = Pointer.current.position.ReadValue();
            move = true;
            turret.Play();
            StartCoroutine("Moving");
        }

        if (context.canceled)
        {
            
            move = false;
            
        }
    }

    private void LevitateCow()
    {
        List<GameObject> cows = new List<GameObject>();

        //update 18/09/2022 - Ivo Seitenfus
        // need to verify gamecontrol for debug purposes
        int rayMultiplier = playerAttributes.RayMultiplier;
        //if (GameControl.Instance != null) rayMultiplier = GameControl.Instance.playerData.rayMultiplier;

        //if (cowsInRay.Count > GameControl.Instance.playerData.rayMultiplier)
        if (cowsInRay.Count > rayMultiplier)
        {

            //for (int i = 0; i <= GameControl.Instance.playerData.rayMultiplier; i++)
            for (int i = 0; i <= rayMultiplier; i++)
            {
                cows.Add(cowsInRay[i]);
            }
        }
        else
        {
            cows = cowsInRay;
        }

        foreach (GameObject c in cows)
        {
            //GameObject cow = cowsInRay[i];
            Rigidbody cowRb = c.GetComponent<Rigidbody>();
            cowRb.AddForce((grabPoint.position - c.transform.position).normalized * playerAttributes.RayForce);
            c.GetComponent<CowBehavior>().OnGrabbed(this);
        }
        //if(Vector3.Distance(grabPoint.position, cow.transform.position) < grabDistance)
        //{
        //    score += 1;
        //    outputText.text = score.ToString();
        //    cowsInRay.Remove(cow);
        //    Destroy(cow);
        //}
    }

    IEnumerator Moving()
    {
        
        while (move)

        {
            
            levelingScript.Leveling();
            
            pressCurrentPosition = Pointer.current.position.ReadValue();
            speedMagnitude = Vector3.Distance(pressStartPosition, pressCurrentPosition);
            if (speedMagnitude >= 100) speedMagnitude = 1f;
            else speedMagnitude /= 100f;
            Vector2 speed = (pressCurrentPosition - pressStartPosition).normalized * speedMagnitude;

            direction = new Vector3(speed.x, 0, speed.y);

            float flyDistance = (direction * playerAttributes.MovementSpeed * Time.deltaTime).magnitude;
            GameControl.Instance.AddFlyDistance(flyDistance);

            transform.Translate(direction * playerAttributes.MovementSpeed * Time.deltaTime);

            TiltFlySaucer();

            PositionFlySaucerTurret();
            
            yield return null;

        }
        
        StartCoroutine("SmoothBreak");
        StartCoroutine("SmoothTilt");
        
        
    }

    void TiltFlySaucer()
    {
        //TILT
        Vector3 tilt = direction * tiltAngle;

        // Rotate the model by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(tilt.z, 0, -tilt.x);

        // Dampen towards the target rotation
        model.rotation = Quaternion.Slerp(model.rotation, target, Time.deltaTime * tiltSpeed);
    }

    void PositionFlySaucerTurret()
    {
        if (direction != Vector3.zero)
        {
            turretTransform.rotation = Quaternion.LookRotation(-direction);
        }
    }


    IEnumerator SmoothBreak()
    {
        Vector3 fromPos = transform.position;
        Vector3 step = direction * playerAttributes.MovementSpeed * deacceleration;

        Vector3 toPos = fromPos + step;
        // toPos = new Vector3(fromPos.x + direction.x, fromPos.y, fromPos.z + direction.y);

        for (float t = 0; t <= 1; t += dampTime)
        {

            transform.position = Vector3.Lerp(fromPos, toPos, t);

            yield return null;
        }

        StartCoroutine("ResetRigidbody");
        turret.Stop();
    }

    IEnumerator SmoothTilt()
    {
        for(float t = 0; t <= 1; t += tiltBackTime)
        {
            Quaternion target = Quaternion.Euler(Vector3.zero);
            model.rotation = Quaternion.Slerp(model.rotation, target, Time.deltaTime * tiltSpeed);
            yield return null;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Cow")
        {
            cowsInRay.Add(other.gameObject);
            SetRay(true);
            StartCoroutine("GrabbingCow");
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cow")
        {
            cowsInRay.Remove(other.gameObject);
            if(cowsInRay.Count <= 0)
            {
                SetRay(false);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        //if (collision.gameObject.tag == "Cow")
        //{
        //    GetCow(collision.gameObject);
        //}

        if (collision.gameObject.tag == "Bound")
        {
            rb.velocity = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3(0f, 0f, 0f);
            

            //ResetRigidbody();
        }



    }

    IEnumerator GrabbingCow()
    {
        
        yield return new WaitForSeconds(0.5f);

        while (cowsInRay.Count > 0)
        {
            yield return new WaitForSeconds(grabRate);
            if (!move)
            {
                var raytractorEmission = rayObjectTractor.emission;
                raytractorEmission.enabled = true;
                yield return new WaitForSeconds(0.8f);

                foreach (GameObject c in cowsInRay)
                {
                    
                    grab = true;
                    //GameObject cow = cowsInRay[i];
                    //Rigidbody cowRb = c.GetComponent<Rigidbody>();
                    //cowRb.AddForce((grabPoint.position - c.transform.position).normalized * playerAttributes.RayForce);
                    c.GetComponent<CowBehavior>().OnGrabbed(this);
                    
                }
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "Bound")
        {
            rb.velocity = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3(0f, 0f, 0f);

            //ResetRigidbody();
        }
    }

    IEnumerator ResetRigidbody()
    {
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = new Vector3(0f, 0f, 0f);
        for (float i = 0; i <= 1; i += Time.deltaTime * 2)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 0.5f);
            yield return null;
        }
    }

    private void SetRay(bool b)
    {
        raio.SetActive(b);
        if (b)
        {
            rayObjectBase.Play();
            //rayObjectRays.Play();


        }
        else
        {
            rayObjectBase.Stop();
            //rayObjectRays.Stop();

        }

        var raytractorEmission = rayObjectTractor.emission;
        raytractorEmission.enabled = false;
    }
    
    public void StartGrabAnimation()
    {
        animator.SetBool("grab", true);
    }

    public void GetCow(GameObject cow)
    {
        

        //score = cow.GetComponent<CowBehavior>().GetReward();

        //GameControl.Instance.AddCoins(cow.GetComponent<CowBehavior>());
        if (GameControl.Instance != null) GameControl.Instance.AddCoins(cow.GetComponent<CowBehavior>());
        cowsInRay.Remove(cow);
        if (cowsInRay.Count <= 0)
        {
            SetRay(false);
        }
        Destroy(cow);
        grab = false;
    }
    public void RayRadius()
    {
        Debug.Log("Button Pressed");
    }


    public void FinishGrabAnimation()
    {
        animator.SetBool("grab", false);
    }
}
