using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamob
{
    public class CloudObstacle : MonoBehaviour
    {
        [SerializeField] private GameObject cloudCollider;
        private float speed;
        [SerializeField] private List<GameObject> thunders;
        [SerializeField] private float minThunderTime;
        [SerializeField] private float maxThunderTime;

        private Vector3 startPoint;
        private Vector3 newPosition;
        private float step;
        private int segment;
        private int segmentsNumber;
        private Vector3[] mullControlPoints;
        [SerializeField] private float mullRange;
        private float[] segmentsDistances;
        private float currentDistance;
        private float cloudSpeed;
        [SerializeField] private List<int> thunderSequence;
        [SerializeField] private AudioSource thunderAudio;

        private void OnEnable()
        {
            Audio.MuteSFX += MuteSFX;
            Audio.UnmuteSFX += UnMuteSFX;
        }

        private void OnDisable()
        {
            Audio.MuteSFX -= MuteSFX;
            Audio.UnmuteSFX -= UnMuteSFX;
        }


        private void Awake()
        {

            if (GameControl.Instance != null)
            {
                if (!GameControl.Instance.playerData.sfx)
                {
                    MuteSFX();
                }
                GameControl.Instance.SetTutorialCloud();
            }
            
            startPoint = transform.position;
            cloudSpeed = Random.Range(3f, 5f);
            speed = Random.Range(3f, 5f);

            segmentsNumber = 4;
            mullControlPoints = new Vector3[segmentsNumber + 3];
            mullControlPoints[0] = startPoint;
            mullControlPoints[1] = startPoint;
            mullControlPoints[2] = new Vector3(Random.Range(transform.position.x - mullRange, transform.position.x + mullRange),
                                                startPoint.y,
                                                Random.Range(transform.position.z - mullRange, transform.position.z + mullRange));
            mullControlPoints[3] = new Vector3(Random.Range(transform.position.x - mullRange, transform.position.x + mullRange),
                                                startPoint.y,
                                                Random.Range(transform.position.z - mullRange, transform.position.z + mullRange));
            mullControlPoints[4] = new Vector3(Random.Range(transform.position.x - mullRange, transform.position.x + mullRange),
                                                startPoint.y,
                                                Random.Range(transform.position.z - mullRange, transform.position.z + mullRange));
            mullControlPoints[5] = mullControlPoints[0];
            mullControlPoints[6] = mullControlPoints[0];
            segment = 1;
            segmentsDistances = new float[segmentsNumber];
            segmentsDistances[0] = Vector3.Distance(mullControlPoints[1], mullControlPoints[2]);
            segmentsDistances[1] = Vector3.Distance(mullControlPoints[2], mullControlPoints[3]);
            segmentsDistances[2] = Vector3.Distance(mullControlPoints[3], mullControlPoints[4]);
            segmentsDistances[3] = Vector3.Distance(mullControlPoints[4], mullControlPoints[1]);

            //StartCoroutine(Move());
            StartCoroutine(MoveCatmull());
            StartCoroutine(CastThunders());
        }

        IEnumerator MoveCatmull()
        {
            while (true)
            {
                currentDistance = segmentsDistances[segment - 1];


                //step += Time.deltaTime;
                step += cloudSpeed * Time.deltaTime / currentDistance;

                newPosition = catmull(step, mullControlPoints[segment - 1], mullControlPoints[segment], mullControlPoints[segment + 1], mullControlPoints[segment + 2]);
                //newDirection = (newPosition - transform.position).normalized;

                //transform.right = newDirection;
                if (step >= 1)
                {
                    segment++;
                    step = 0;
                    if (segment > segmentsNumber)
                        segment = 1;

                }

                transform.localPosition = newPosition;

                yield return null;
            }

        }

        Vector3 catmull(float _t, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 controlPoint3, Vector3 controlPoint4)
        {
            Vector3 newVec;
            float t2 = _t * _t;
            float t3 = _t * _t * _t;


            newVec.x = (float)0.5 * ((2 * controlPoint2.x) + ((controlPoint3.x - controlPoint1.x) * _t) + (((2 * controlPoint1.x) - (5 * controlPoint2.x) + (4 * controlPoint3.x) - (controlPoint4.x)) * t2) + (((-controlPoint1.x) + (3 * controlPoint2.x) - (3 * controlPoint3.x) + (controlPoint4.x)) * t3));
            newVec.y = (float)0.5 * ((2 * controlPoint2.y) + ((controlPoint3.y - controlPoint1.y) * _t) + (((2 * controlPoint1.y) - (5 * controlPoint2.y) + (4 * controlPoint3.y) - (controlPoint4.y)) * t2) + (((-controlPoint1.y) + (3 * controlPoint2.y) - (3 * controlPoint3.y) + (controlPoint4.y)) * t3));

            newVec.z = (float)0.5 * ((2 * controlPoint2.z) + ((controlPoint3.z - controlPoint1.z) * _t) + (((2 * controlPoint1.z) - (5 * controlPoint2.z) + (4 * controlPoint3.z) - (controlPoint4.z)) * t2) + (((-controlPoint1.z) + (3 * controlPoint2.z) - (3 * controlPoint3.z) + (controlPoint4.z)) * t3));


            return newVec;
        }

        IEnumerator Move()
        {
            float turn = 5f;
            float step = speed * Time.deltaTime;
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (turn <= 0f)
                {
                    for (float i = -5f; i <= 5f; i += step)
                    {
                        transform.position += new Vector3(step, 0, 0);
                        turn = i;
                        yield return null;
                    }
                }
                else
                {
                    for (float i = 5f; i >= -5f; i -= step)
                    {
                        transform.position -= new Vector3(step, 0, 0);
                        turn = i;
                        yield return null;
                    }
                }
            }
        }

        IEnumerator CastThunders()
        {
            int waitTime = 0;

            while (true)
            {
                waitTime = Random.Range(2, 5);
                yield return new WaitForSeconds(waitTime);

                cloudCollider.SetActive(true);
                if (thunderSequence.Count > 0)
                {
                    for (int i = 0; i < thunderSequence.Count; i++)
                    {
                        yield return StartCoroutine(LightOnThunder(thunderSequence[i]));
                        yield return StartCoroutine(LightOffThunder(thunderSequence[i]));
                    }
                }
                else
                {
                    yield return StartCoroutine(LightOnThunder(0));
                    yield return StartCoroutine(LightOffThunder(0));
                    yield return StartCoroutine(LightOnThunder(1));
                    yield return StartCoroutine(LightOffThunder(1));
                    yield return StartCoroutine(LightOnThunder(2));
                    yield return StartCoroutine(LightOffThunder(2));
                }

                cloudCollider.SetActive(false);

            }
        }

        IEnumerator LightOnThunder(int thunder)
        {
            float thunderTime = Random.Range(minThunderTime, maxThunderTime);
            thunders[thunder].SetActive(true);
            thunderAudio.Play();
            yield return new WaitForSeconds(thunderTime);
        }

        IEnumerator LightOffThunder(int thunder)
        {
            float thunderTime = Random.Range(minThunderTime, maxThunderTime);
            thunders[thunder].SetActive(false);
            yield return new WaitForSeconds(thunderTime);
        }
        void MuteSFX()
        {
            thunderAudio.mute = true;
        }

        void UnMuteSFX()
        {
            thunderAudio.mute = false;
        }
    }

    
}
