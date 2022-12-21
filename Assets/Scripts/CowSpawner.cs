using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamob
{
    public class CowSpawner : MonoBehaviour
    {
        [SerializeField] private CowData[] cowPrefabs;
        [SerializeField] private List<CowData> loadedCows;
        [SerializeField] private Vector2 distanceLimits;
        [SerializeField] private int maxCows;





        public void Initialize(int numCows)
        {
            if (GameControl.Instance != null)
            {
                loadedCows = GameControl.Instance.PurchasedCows();
            }
            else
            {
                foreach (CowData cow in cowPrefabs)
                {
                    loadedCows.Add(cow);
                }
            }
            maxCows = numCows;
            StartCoroutine(InstantiateCows());
        }



        IEnumerator InstantiateCows()
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < maxCows; i++)
            {
                Vector3 position = GetDestinationPoint();
                //Instantiate(cowPrefabs[Random.Range(0, cowPrefabs.Length)], position, Quaternion.identity);
                Instantiate(loadedCows[Random.Range(0, loadedCows.Count)].gamePrefab, position, Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
            }
        }

        private Vector3 GetDestinationPoint()
        {
            RaycastHit hit;
            bool blocked = true;
            Vector3 dest = new Vector3();
            while (blocked)
            {
                Vector2 randomDir = Random.insideUnitCircle;
                dest = transform.position + new Vector3(randomDir.x * Random.Range(distanceLimits.x, distanceLimits.y), 0, randomDir.y * Random.Range(distanceLimits.x, distanceLimits.y));
                float sightDistance = (dest - transform.position).magnitude;
                Vector3 dirDiff = (dest - transform.position).normalized;
                Vector3 direction = new Vector3(dirDiff.x, 0, dirDiff.z);

                Ray ray = new Ray(transform.position, direction);

                if (!Physics.Raycast(ray, out hit, sightDistance))
                {


                    blocked = false;
                }

            }
            return dest;
        }
    }
}