using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamob
{
    public class Cloud : MonoBehaviour
    {
        [SerializeField] private Renderer objRenderer;
        private float alphaSpeed;
        private float objectAlpha;
        private float step;
        //[SerializeField] private Vector3 endPoint;
        private Vector3 startPoint;
        private Vector3 newPosition;
        [SerializeField] private int alphaChance;
        [SerializeField] private int cloudMoveChance;

        private int segment;
        private int segmentsNumber;
        private Vector3[] mullControlPoints;
        [SerializeField] private float mullRange;


        private void Awake()
        {
            objectAlpha = 1f;
            objRenderer = gameObject.GetComponent<Renderer>();
            alphaSpeed = Random.Range(0.005f, 0.01f);

            //endPoint = new Vector3(Random.Range(-1.0f, 1.0f), transform.position.y, Random.Range(-1.0f, 1.0f));
            //direcao = new Vector3(endPoint.x - transform.position.x, endPoint.y - transform.position.y, endPoint.z - transform.position.z).normalized;
            newPosition = Vector3.zero;
            startPoint = transform.localPosition;


            segmentsNumber = 4;
            mullControlPoints = new Vector3[segmentsNumber + 3];
            mullControlPoints[0] = startPoint;
            mullControlPoints[1] = startPoint;
            mullControlPoints[2] = new Vector3(Random.Range(transform.localPosition.x - mullRange, transform.localPosition.x + mullRange),
                                                Random.Range(transform.localPosition.y - mullRange, transform.localPosition.y + mullRange),
                                                Random.Range(transform.localPosition.z - mullRange, transform.localPosition.z + mullRange));
            mullControlPoints[3] = new Vector3(Random.Range(transform.localPosition.x - mullRange, transform.localPosition.x + mullRange),
                                                Random.Range(transform.localPosition.y - mullRange, transform.localPosition.y + mullRange),
                                                Random.Range(transform.localPosition.z - mullRange, transform.localPosition.z + mullRange));
            mullControlPoints[4] = new Vector3(Random.Range(transform.localPosition.x - mullRange, transform.localPosition.x + mullRange),
                                                Random.Range(transform.localPosition.y - mullRange, transform.localPosition.y + mullRange),
                                                Random.Range(transform.localPosition.z - mullRange, transform.localPosition.z + mullRange));
            mullControlPoints[5] = mullControlPoints[0];
            mullControlPoints[6] = mullControlPoints[0];
            segment = 1;

            int alphaLotery = Random.Range(1, 11);
            int cloudMoveLotery = Random.Range(1, 11);

            if (alphaChance >= alphaLotery) StartCoroutine(OscilateRenderAlpha());

            if (cloudMoveChance >= cloudMoveLotery) StartCoroutine(MoveCatmull());

        }

        IEnumerator OscilateRenderAlpha()
        {

            while (true)
            {

                while (objRenderer.isVisible)
                {

                    yield return new WaitForSeconds(Random.Range(1, 3));
                    if (objectAlpha >= 0.5f)
                    {

                        for (float i = 1.0f; i >= 0.1f; i -= alphaSpeed)
                        {

                            Color rendererColor = objRenderer.material.GetColor("_TintColor");
                            rendererColor.a = i;
                            objectAlpha = i;
                            objRenderer.material.SetColor("_TintColor", rendererColor);
                            yield return null;
                        }
                    }
                    else
                    {

                        for (float i = 0.1f; i <= 1.0f; i += alphaSpeed)
                        {
                            Color rendererColor = objRenderer.material.GetColor("_TintColor");
                            rendererColor.a = i;
                            objectAlpha = i;
                            objRenderer.material.SetColor("_TintColor", rendererColor);
                            yield return null;
                        }
                    }

                }
                yield return null;
            }


        }


        IEnumerator MoveCatmull()
        {
            while (true)
            {
                step += Time.deltaTime;

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


    }
}