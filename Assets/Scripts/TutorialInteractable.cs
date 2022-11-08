using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialInteractable : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Tutorial tutorial;
    public void OnPointerEnter(PointerEventData eventData)
    {
        tutorial.OnPointerEnter();
    }
}
