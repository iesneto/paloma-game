using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableInterface : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float minScale;
    public float maxScale;
    public float scaleSpeed;
    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine("ScaleDownGameObject");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine("ScaleUpGameObject");
    }


    private void OnDisable()
    {
        StopAllCoroutines();
        gameObject.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
    }

    IEnumerator ScaleDownGameObject()
    {
        for(float i = maxScale; i >= minScale; i -= scaleSpeed*Time.deltaTime)
        {
            // Vector3 newscale = gameObject.transform.localScale - new Vector3(-)
            gameObject.transform.localScale = new Vector3(i,i,i);
            yield return null;
        }
        gameObject.transform.localScale = new Vector3(minScale, minScale, minScale);
    }

    IEnumerator ScaleUpGameObject()
    {
        for (float i = minScale; i <= maxScale; i += scaleSpeed*Time.deltaTime)
        {
            // Vector3 newscale = gameObject.transform.localScale - new Vector3(-)
            gameObject.transform.localScale = new Vector3(i, i, i);
            yield return null;
        }
        gameObject.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
    }
}
