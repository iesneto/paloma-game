using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundAnimation : MonoBehaviour
{
    private float scrollSpeed;
    private float scrollLength;
    Vector2 startPos;

    private Image image02;
    private Rect image02Rect;

    private void Start()
    {
        scrollSpeed = -Screen.width/10f;
        startPos = transform.position;
        image02 = transform.Find("BGImage02").gameObject.GetComponent<Image>();
        image02Rect = image02.rectTransform.rect;
        image02.rectTransform.localPosition = new Vector3(-image02Rect.width, image02.rectTransform.localPosition.y, image02.rectTransform.localPosition.z);
        scrollLength = Screen.width;
    }

    private void Update()
    {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed, scrollLength);
        transform.position = startPos + Vector2.right * newPos;
    }
}
