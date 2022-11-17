using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.Events;
using System;

public class InteractableInterface : Selectable, IPointerDownHandler, IPointerClickHandler, IPointerExitHandler
{
    [Serializable]
    /// <summary>
    /// Function definition for a button click event.
    /// </summary>
    public class ButtonClickedEvent : UnityEvent { }

    // Event delegates triggered on click.
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

    protected InteractableInterface()
    { }

    public ButtonClickedEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }

    private void Press()
    {
        if (!IsActive() || !IsInteractable())
            return;

        UISystemProfilerApi.AddMarker("Button.onClick", this);
        m_OnClick.Invoke();
    }

    [SerializeField] private Transform m_transform;
    private float currentScale = 1f;
    private float maxScale = 1f;
    private float minScale = 0.5f;
    private float scaleSpeed = 4f;
    private bool up, down, exit;
    public override void OnPointerDown(PointerEventData eventData)
    {
        down = true;
        StartCoroutine("ScaleDownGameObject");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        up = true;
        StartCoroutine("ScaleUpGameObject");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        exit = true;
        StartCoroutine("ScaleUpGameObject");
    }

    protected override void OnDisable()
    {
        StopAllCoroutines();
        gameObject.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
    }

    IEnumerator ScaleDownGameObject()
    {
        for(float i = currentScale; i >= minScale; i -= scaleSpeed*Time.deltaTime)
        {
            // Vector3 newscale = gameObject.transform.localScale - new Vector3(-)
            m_transform.localScale = new Vector3(i,i,i);
            currentScale = i;
            yield return null;
            if (up || exit) break;
        }
        //gameObject.transform.localScale = new Vector3(minScale, minScale, minScale);
    }

    IEnumerator ScaleUpGameObject()
    {
        for (float i = currentScale; i <= maxScale; i += scaleSpeed*Time.deltaTime)
        {
            // Vector3 newscale = gameObject.transform.localScale - new Vector3(-)
            m_transform.localScale = new Vector3(i, i, i);
            currentScale = i;
            yield return null;
        }
        //gameObject.transform.localScale = new Vector3(maxScale, maxScale, maxScale);

        if (up) Press();

        up = exit = false;
    }

}
