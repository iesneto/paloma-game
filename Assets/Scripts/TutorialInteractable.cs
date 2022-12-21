using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gamob
{
    public class TutorialInteractable : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Tutorial tutorial;
        public void OnPointerDown(PointerEventData eventData)
        {
            tutorial.OnPointerEnter();
        }
    }
}