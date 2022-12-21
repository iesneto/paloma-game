using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamob
{
    public class CameraEffects : MonoBehaviour
    {
        [SerializeField] private float magnitude;
        [SerializeField] private float duration;
        private CameraFollow cameraFollowScript;
        private void Awake()
        {
            cameraFollowScript = gameObject.GetComponent<CameraFollow>();
        }

        public IEnumerator Shake()
        {
            cameraFollowScript.Shock(true);
            Vector3 originalPosition = transform.position;
            float elapsed = 0f;

            while(elapsed <= duration)
            {
                float x = originalPosition.x + Random.Range(-1f, 1f) * magnitude;
                float y = originalPosition.y + Random.Range(-1f, 1f) * magnitude;

                transform.position = new Vector3(x, y, originalPosition.z);

                elapsed += Time.deltaTime;

                yield return null;
                
            }

            transform.localPosition = originalPosition;
            cameraFollowScript.Shock(false);
        }
    }
}
