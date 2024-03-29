﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.UI;
using UnityEngine.ParticleSystemJobs;

namespace Gamob
{
    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField] private bool locked;
        [SerializeField] private bool shocked;
        private CameraEffects cameraEffects;
        private Camera camera;
        public float grabRate;
        public bool instantGrab;
        [SerializeField] private PlayerAttributes playerAttributes;
        [SerializeField] private ObjectPositioning levelingScript;
        [SerializeField] private GameObject raio;
        [SerializeField] private ParticleSystem rayObjectBase;
        [SerializeField] private ParticleSystem rayObjectRays;
        [SerializeField] private ParticleSystem rayObjectTractor;
        [SerializeField] private ParticleSystem turret;
        [SerializeField] private Transform turretTransform;
        [SerializeField] private float turretTiltAmount;
        //[SerializeField] private Vector3 turretDirection;
        //[SerializeField] private float turretRotationSpeed;

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
        [SerializeField] private Animator animator;
        [SerializeField] private Transform coinsPosition;
        [SerializeField] private Transform experiencePosition;
        [SerializeField] private GameObject coinsPickUpPrefab;
        [SerializeField] private GameObject experiencePickUpPrefab;
        [SerializeField] private FlyingSaucerAudio flyingSaucerAudio;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private float flyingDistance;
        [SerializeField] private float flyingDistanceThreshold;
        [SerializeField] private GameObject modelPlaceholder;
        private bool grabAnimation;
        //[SerializeField] private List<GameObject> flyingSaucerPrefabs;

        //public Text outputText;
        private Rigidbody rb;


        // private Controls controls;
        //private PlayerControls controls;

        private void Awake()
        {
            //controls = new PlayerControls();
            playerAttributes = GetComponent<PlayerAttributes>();
            playerAttributes.InitializeAttributes();
            //animator = model.gameObject.GetComponentInChildren<Animator>();
            animator = model.gameObject.GetComponent<Animator>();

            levelingScript = GetComponent<ObjectPositioning>();

            raio.SetActive(false);
            //flyingSaucerAudio.StartFlyingSaucer();


            rb = gameObject.GetComponent<Rigidbody>();

            LoadFlyingSaucerModel();
        }

        void LoadFlyingSaucerModel()
        {
            if (GameControl.Instance != null)
            {
                Instantiate(GameControl.Instance.FlyingSaucerModelByIndex(GameControl.Instance.playerData.flyingSaucerModelId), model);
            }
            else
            {
                Instantiate(modelPlaceholder, model);
            }
        }

        public void Lock()
        {
            locked = true;
        }

        public void Unlock()
        {
            locked = false;
        }

        private void OnEnable()
        {
            

        }



        private void OnDisable()
        {

            
        }

        public void OnMoveTouch(InputAction.CallbackContext context)
        {
            // TODO: Code to implement TouchScreen Move
            //if (context.performed)
            //{
            //    // We want to hit only the Floor layer
            //    int layermask = 1 << LayerMask.NameToLayer("Floor");
            //    RaycastHit hit;
            //    Ray ray = camera.ScreenPointToRay(playerInput.actions["TouchMove"].ReadValue<Vector2>());

            //    if (Physics.Raycast(ray, out hit, 100, layermask))
            //    {
            //        Transform objectHit = hit.transform;
            //        Debug.DrawRay(camera.gameObject.transform.position, hit.point - camera.gameObject.transform.position, Color.blue, 5);
            //    }
                
            //}
        }

        

        private void Update()
        {

            if (locked || shocked) return;


            Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();

            if (input != Vector2.zero)
            {
                if (!move)
                {
                    flyingSaucerAudio.PlayMove();
                    move = true;
                    turret.Play();
                    StartCoroutine("Moving");
                }

            }
            else
            {
                if (move)
                {
                    //flyingSaucerAudio.StopMove();
                    //move = false;
                    //turret.Stop();
                    StopMove();
                }
            }
        }

        private void LevitateCow()
        {
            List<GameObject> cows = new List<GameObject>();

            int rayMultiplier = playerAttributes.RayMultiplier;

            if (cowsInRay.Count > rayMultiplier)
            {

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

        }
        IEnumerator TouchMoving()
        {

            while (move)

            {

                levelingScript.Leveling();

                //pressCurrentPosition = Pointer.current.position.ReadValue();
                //speedMagnitude = Vector3.Distance(pressStartPosition, pressCurrentPosition);
                //if (speedMagnitude >= 100) speedMagnitude = 1f;
                //else speedMagnitude /= 100f;
                //Vector2 speed = (pressCurrentPosition - pressStartPosition).normalized * speedMagnitude;

                //direction = new Vector3(speed.x, 0, speed.y);
                Vector2 input = playerInput.actions["TouchMove"].ReadValue<Vector2>();



                direction = new Vector3(input.x, 0, input.y);

                //float flyDistance = (direction * playerAttributes.MovementSpeed * Time.deltaTime).magnitude;


                Vector3 previousPosition = transform.position;
                transform.Translate(direction * playerAttributes.MovementSpeed * Time.deltaTime);


                TiltFlySaucer();

                PositionFlySaucerTurret();

                yield return null;

                Vector3 currentPosition = transform.position;
                flyingDistance = Vector3.Distance(currentPosition, previousPosition);
                if (GameControl.Instance != null && flyingDistance >= flyingDistanceThreshold)
                {
                    GameControl.Instance.AddFlyDistance(flyingDistance);
                }

            }
            StartCoroutine("SmoothBreak");
            StartCoroutine("SmoothTilt");


        }

        IEnumerator Moving()
        {

            while (move && !locked)

            {

                levelingScript.Leveling();

                //pressCurrentPosition = Pointer.current.position.ReadValue();
                //speedMagnitude = Vector3.Distance(pressStartPosition, pressCurrentPosition);
                //if (speedMagnitude >= 100) speedMagnitude = 1f;
                //else speedMagnitude /= 100f;
                //Vector2 speed = (pressCurrentPosition - pressStartPosition).normalized * speedMagnitude;

                //direction = new Vector3(speed.x, 0, speed.y);
                Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();

                direction = new Vector3(input.x, 0, input.y);

                //float flyDistance = (direction * playerAttributes.MovementSpeed * Time.deltaTime).magnitude;


                Vector3 previousPosition = transform.position;
                transform.Translate(direction * playerAttributes.MovementSpeed * Time.deltaTime);


                TiltFlySaucer();

                PositionFlySaucerTurret();

                yield return null;

                Vector3 currentPosition = transform.position;
                flyingDistance = Vector3.Distance(currentPosition, previousPosition);
                if (GameControl.Instance != null && flyingDistance >= flyingDistanceThreshold)
                {
                    GameControl.Instance.AddFlyDistance(flyingDistance);
                }

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
                //turretTransform.rotation = Quaternion.LookRotation(-direction);
                Vector3 turretLookRotation = new Vector3(-direction.x, turretTiltAmount, -direction.z);
                turretTransform.rotation = Quaternion.LookRotation(turretLookRotation);
            }
        }

        void TurretTiltBack()
        {
            if (direction != Vector3.zero)
            {
                Vector3 turretLookRotation = new Vector3(-direction.x, 0, -direction.z);
                turretTransform.rotation = Quaternion.LookRotation(turretLookRotation);
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
                if (move) break;
            }

            StartCoroutine("ResetRigidbody");

        }

        IEnumerator SmoothTilt()
        {
            for (float t = 0; t <= 1; t += tiltBackTime)
            {
                Quaternion target = Quaternion.Euler(Vector3.zero);
                model.rotation = Quaternion.Slerp(model.rotation, target, Time.deltaTime * tiltSpeed);

                TurretTiltBack();

                yield return null;
                if (move) break;
            }
        }
        public void OnTriggerEnter(Collider other)
        {
            if (locked) return;

            if (other.tag == "Cow")
            {
                cowsInRay.Add(other.gameObject);
                SetRay(true);
                StartCoroutine("GrabbingCow");
            }
            else if (other.tag == "Cloud" && !shocked)
            {
                shocked = true;                
                StartCoroutine(ShockFlyingSaucer());
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.tag == "Cow")
            {
                cowsInRay.Remove(other.gameObject);
                if (cowsInRay.Count <= 0)
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

        void StopMove()
        {
            flyingSaucerAudio.StopMove();
            move = false;
            turret.Stop();
        }
        IEnumerator ShockFlyingSaucer()
        {
            GameControl.Instance.Shocked();
            StopMove();
            flyingSaucerAudio.PlayShocked();
            StartCoroutine(cameraEffects.Shake());
            StartCoroutine(GameControl.Instance.GameControlUI().ThunderFrame());
            yield return new WaitForSeconds(2);
            shocked = false;
        }

        IEnumerator GrabbingCow()
        {
            int rayMultiplier = playerAttributes.RayMultiplier;
            int maxCowsToGrab = 0;
            if (cowsInRay.Count >= rayMultiplier)
            {
                maxCowsToGrab = rayMultiplier;
            }
            else
            {
                maxCowsToGrab = cowsInRay.Count;
            }

            while (cowsInRay.Count > 0)
            {
                for (int i = 0; i < maxCowsToGrab; i++)
                {
                    if (cowsInRay.Count > i)
                    {
                        GameObject cow = cowsInRay[i];
                        grab = true;
                        cow.GetComponent<CowBehavior>().OnGrabbed(this);
                    }


                }
                yield return new WaitForSeconds(grabRate);
                //if (!move || instantGrab)
                //{

                //    if(cowsInRay.Count > 0)
                //    {




                //    }
                //}
            }
        }

        void TurnOnRayTractor()
        {
            var raytractorEmission = rayObjectTractor.emission;
            raytractorEmission.enabled = true;
        }

        void TurnOffRayTractor()
        {
            var raytractorEmission = rayObjectTractor.emission;
            raytractorEmission.enabled = false;
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
                TurnOnRayTractor();
                flyingSaucerAudio.PlayRay();
                //rayObjectRays.Play();


            }
            else
            {
                rayObjectBase.Stop();
                //rayObjectRays.Stop();
                TurnOffRayTractor();
                flyingSaucerAudio.StopRay();
            }


        }

        public void StartGrabAnimation()
        {
            if (!animator.GetBool("grab"))
            {
                animator.SetBool("grab", true);
                Invoke("FinishGrabAnimation", 0.35f);
            }
            flyingSaucerAudio.PlayGrab();
        }


        public void FinishGrabAnimation()
        {
            animator.SetBool("grab", false);
        }

        public void GetCow(GameObject cow)
        {


            //score = cow.GetComponent<CowBehavior>().GetReward();

            //GameControl.Instance.AddCoins(cow.GetComponent<CowBehavior>());
            if (GameControl.Instance != null) GameControl.Instance.AddCowCoinsAndXp(cow.GetComponent<CowBehavior>());
            PlayPickUpReward(cow);
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
            //Debug.Log("Button Pressed");
        }

        void PlayPickUpReward(GameObject cow)
        {
            CowBehavior thisCow = cow.GetComponent<CowBehavior>();
            int xpValue = thisCow.GetExperience();
            int coinValue = thisCow.GetReward();
            GameObject coinObject = Instantiate(coinsPickUpPrefab, coinsPosition);
            GameObject xpObject = Instantiate(experiencePickUpPrefab, experiencePosition);

            coinObject.GetComponent<PickUpUI>().Setup(coinValue);
            xpObject.GetComponent<PickUpUI>().Setup(xpValue);
            flyingSaucerAudio.PlayReward();
        }

        public void SetCameraEffectReference(CameraEffects script)
        {
            cameraEffects = script;
            camera = cameraEffects.gameObject.GetComponent<Camera>();
        }

    }
}
