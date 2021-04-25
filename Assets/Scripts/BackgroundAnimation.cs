using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundAnimation : MonoBehaviour
{
    private float scrollSpeed;
    private float scrollLength;
    Vector2 startPos;

    //private Image image02;
    [SerializeField]private GameObject objectImage02;
    //private Rect objectImage02Rect;
    private RectTransform objectImage02RectTransform;
    private Rect objectImage02Rect;

    private void Start()
    {
        scrollSpeed = -Screen.width/10f;
        startPos = transform.position;
        //image02 = transform.Find("BGImage02").gameObject.GetComponent<Image>();
        objectImage02RectTransform = objectImage02.GetComponent<RectTransform>();
        objectImage02Rect = objectImage02RectTransform.rect;
        //image02.rectTransform.localPosition = new Vector3(-image02Rect.width, image02.rectTransform.localPosition.y, image02.rectTransform.localPosition.z);
        objectImage02RectTransform.localPosition = new Vector3(-objectImage02Rect.width, objectImage02RectTransform.localPosition.y, objectImage02RectTransform.localPosition.z);
        scrollLength = Screen.width;
    }

    private void Update()
    {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed, scrollLength);
        transform.position = startPos + Vector2.right * newPos;
    }
}
