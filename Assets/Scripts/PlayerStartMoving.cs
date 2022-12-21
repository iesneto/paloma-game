using UnityEngine.EventSystems;
using UnityEngine;

namespace Gamob
{
    public class PlayerStartMoving : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {

            GameControl.Instance.StartMoving();
        }
    }
}